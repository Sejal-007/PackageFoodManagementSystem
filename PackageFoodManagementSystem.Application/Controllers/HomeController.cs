using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _db;

        public HomeController(IUserService userService, ApplicationDbContext db)
        {
            _userService = userService;
            _db = db;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult SignIn() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserAuthentication loginUser)
        {
            if (!ModelState.IsValid) return View(loginUser);

            var user = await _db.UserAuthentications
                .FirstOrDefaultAsync(u => u.Email == loginUser.Email);

            if (user == null || !PasswordHelper.VerifyPassword(loginUser.Password, user.Password))
            {
                TempData["ErrorMessage"] = "Incorrect Login Credentials";
                return View(loginUser);
            }

            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserMobile", user.MobileNumber);
            HttpContext.Session.SetString("UserRole", user.Role);

            TempData["SuccessMessage"] = $"Welcome, {user.Name}!";

            return user.Role switch
            {
                "Admin" => RedirectToAction("AdminDashboard"),
                "StoreManager" => RedirectToAction("ManagerDashboard"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthentication user)
        {
            if (!ModelState.IsValid) return View(user);

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                user.Password = PasswordHelper.HashPassword(user.Password);
                user.Role = "User";
                _db.UserAuthentications.Add(user);
                await _db.SaveChangesAsync();

                var customer = new Customer
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.MobileNumber,
                    Status = "Active",
                    Addresses = new List<CustomerAddress>()
                };

                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return RedirectToAction(nameof(SignIn));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Sync Error: {ex.InnerException?.Message ?? ex.Message}";
                return View(user);
            }
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalCustomers = await _db.UserAuthentications.CountAsync(u => u.Role == "User");
            ViewBag.TotalStoreManagers = await _db.UserAuthentications.CountAsync(u => u.Role == "StoreManager");
            ViewBag.TotalOrders = await _db.Orders.CountAsync();
            return View();
        }

        public IActionResult ManagerDashboard() => View();

        public async Task<IActionResult> Users()
        {
            var users = await _db.UserAuthentications.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserAuthentication user)
        {
            if (!ModelState.IsValid)
                return View("Users", await _db.UserAuthentications.ToListAsync());

            user.Password = PasswordHelper.HashPassword(user.Password);
            _db.UserAuthentications.Add(user);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "User added successfully!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserAuthentication user)
        {
            if (!ModelState.IsValid)
            {
                var users = await _db.UserAuthentications.ToListAsync();
                return View("Users", users);
            }

            var existingUser = await _db.UserAuthentications.FindAsync(user.Id);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.MobileNumber = user.MobileNumber;
            existingUser.Role = user.Role;

            if (!string.IsNullOrEmpty(user.Password))
                existingUser.Password = PasswordHelper.HashPassword(user.Password);

            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.UserAuthentications.FindAsync(id);
            if (user == null) return NotFound();

            var customers = _db.Customers.Where(c => c.UserId == id);
            _db.Customers.RemoveRange(customers);

            _db.UserAuthentications.Remove(user);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction(nameof(Users));
        }

        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult AdminInventory() => View();
        public IActionResult Report() => View();
        public IActionResult Stores() => View();

        public IActionResult Welcome()
        {
            return View();
        }
    }
}