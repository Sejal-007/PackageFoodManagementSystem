using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Repository.Data;
using System.Threading.Tasks;
using PackageFoodManagementSystem.Services.Interfaces;

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

        // --- Sign In ---
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

            TempData["SuccessMessage"] = "Login Successful";

            return user.Role switch
            {
                "Admin" => RedirectToAction("AdminDashboard"),
                "StoreManager" => RedirectToAction("ManagerDashboard"),
                _ => RedirectToAction(nameof(Index))
            };
        }

        // --- Sign Up ---
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthentication user)
        {
            if (!ModelState.IsValid) return View(user);

            var existingUser = await _db.UserAuthentications
                .FirstOrDefaultAsync(u => u.Email == user.Email);


            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Email already exists. Please use a different email.";
                return View(user);
            }

            await _userService.CreateUserAsync(
                user.Name,
                user.MobileNumber,
                user.Email,
                user.Password);

            TempData["SuccessMessage"] = "SignUp Successful";
            return RedirectToAction(nameof(SignIn));
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

        // --- User Management (CRUD) ---
        public async Task<IActionResult> Users()
        {
            var users = await _db.UserAuthentications.ToListAsync();
            return View(users); // ✅ pass model to view
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
                return View("Users", users); // show the same page with errors
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

            _db.UserAuthentications.Remove(user);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction(nameof(Users));
        }

        // --- Other Pages ---
        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult AdminInventory() => View();
        public IActionResult Report() => View();
        public IActionResult Stores() => View();

        public IActionResult Welcome()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }
    }
}
