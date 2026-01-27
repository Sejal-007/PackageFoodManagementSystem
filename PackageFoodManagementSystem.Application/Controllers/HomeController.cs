using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _db;

        // Unified single constructor
        public HomeController(IUserService userService, ApplicationDbContext db)
        {
            _userService = userService;
            _db = db;
        }

        public IActionResult Index() => View();

        public IActionResult Welcome()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        // --- SIGN IN LOGIC ---
        [HttpGet]
        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var user = await _db.UserAuthentications
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                // 1. Create Claims for Authentication Cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // 2. Sign In (Sets Cookie)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // 3. STORE IN SESSION: Fixes the "Guest" issue in your dashboard
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPhone", user.MobileNumber ?? "");

                // Redirect based on Role
                if (user.Role == "Admin") return RedirectToAction("AdminDashboard");
                if (user.Role == "StoreManager") return RedirectToAction("ManagerDashboard");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        // --- SIGN UP LOGIC ---
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
                // 1. Create User
                user.Password = PasswordHelper.HashPassword(user.Password);
                user.Role = "User";
                _db.UserAuthentications.Add(user);
                await _db.SaveChangesAsync();

                // 2. Create Customer Profile (Sync)
                var customer = new Customer
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.MobileNumber,
                    Status = "Active"
                };

                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                TempData["SuccessMessage"] = "Account created! Please sign in.";
                return RedirectToAction(nameof(SignIn));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Sync Error: {ex.InnerException?.Message ?? ex.Message}";
                return View(user);
            }
        }

        // --- DASHBOARDS ---
        public IActionResult Dashboard() => View();

        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalCustomers = await _db.UserAuthentications.CountAsync(u => u.Role == "User");
            ViewBag.TotalStoreManagers = await _db.UserAuthentications.CountAsync(u => u.Role == "StoreManager");
            ViewBag.TotalOrders = await _db.Orders.CountAsync();
            return View();
        }

        public IActionResult ManagerDashboard() => View();

        // --- USER MANAGEMENT (CRUD) ---
        public async Task<IActionResult> Users()
        {
            var users = await _db.UserAuthentications.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserAuthentication user)
        {
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

        // --- STATIC PAGES ---
        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
    }
}