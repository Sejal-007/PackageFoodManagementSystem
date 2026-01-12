
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Application.Helpers;
using PackageFoodManagementSystem.Application.Models;
using PackageFoodManagementSystem.Application.Data;
using PackageFoodManagementSystem.Services;
using System.Threading.Tasks;

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

            // ✅ Role-based redirection
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("AdminDashboard", "Home");
                case "StoreManager":
                    return RedirectToAction("Home", "StoreManager");
                default:
                    return RedirectToAction(nameof(Index));
            }
        }




        // --- Sign Up ---
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthentication user)
        {
            if (!ModelState.IsValid) return View(user);
            var existingUser = await _db.UserAuthentications.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            { // ❌ Email already exists → show error
              TempData["ErrorMessage"] = "Email already exists. Please use a different email."; 
              return View(user); // stay on SignUp page
            }

                var result = await _userService.CreateUserAsync(
                user.Name,
                user.MobileNumber,
                user.Email,
                user.Password);

            // ✅ SignUp success → set success message
            TempData["SuccessMessage"] = "SignUp Successful"; 
            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> AdminDashboard()
        {
            // Count all users with Role = "User"
            var totalCustomers = await _db.UserAuthentications
                .CountAsync(u => u.Role == "User");

            var totalStoreManagers = await _db.UserAuthentications
                .CountAsync(u => u.Role == "StoreManager");

            // Count active/inactive stores if you have a Stores table
            //var activeStores = await _db.Stores.CountAsync(s => s.IsActive);
            //var inactiveStores = await _db.Stores.CountAsync(s => !s.IsActive);

            // Example: orders, batches, etc. if you have those tables
            var totalOrders = await _db.Orders.CountAsync();
            //var totalBatches = await _db.Batches.CountAsync();

            // Pass values to the view using ViewBag
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalStoreManagers = totalStoreManagers;
            //ViewBag.InactiveStores = inactiveStores;
            ViewBag.TotalOrders = totalOrders;
            //ViewBag.TotalBatches = totalBatches;

            return View();
        }



        public IActionResult ManagerDashboard() => View();

        public IActionResult Users() => View();

        public IActionResult AboutUs() => View();

        public IActionResult ContactUs() => View();

        public IActionResult AdminInventory()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult Stores()
        {
            return View();
        }
        public IActionResult Welcome()
        {
            // If already logged in, skip the welcome page and go to the store
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
