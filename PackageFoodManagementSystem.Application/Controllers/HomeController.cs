using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;

        public HomeController(IUserService userService, IConfiguration config, ApplicationDbContext db)
        {
            _userService = userService;
            _config = config;
            _db = db;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index() => View();

        public IActionResult Welcome()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        [HttpGet]
        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);

                if (user.Role == "Admin") return RedirectToAction("AdminDashboard");
                if (user.Role == "StoreManager") return RedirectToAction("Home", "StoreManager");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        [HttpPost]
        [Route("api/signin")]
        [AllowAnonymous]
        public async Task<IActionResult> ApiSignIn(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                var token = JwtHelper.GenerateJwtToken(user, _config);
                return Ok(new { Token = token, UserId = user.Id, Name = user.Name });
            }
            return Unauthorized("Invalid email or password.");
        }

        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthentication user)
        {
            if (!ModelState.IsValid) return View(user);

            try
            {
                // Create user using service
                var userId = await _userService.CreateUserAsync(user.Name, user.MobileNumber, user.Email, user.Password);

                TempData["SuccessMessage"] = "Account created! Please sign in.";
                return RedirectToAction(nameof(SignIn));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(user);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalCustomers = await _db.UserAuthentications.CountAsync(u => u.Role == "User");
            ViewBag.TotalStoreManagers = await _db.UserAuthentications.CountAsync(u => u.Role == "StoreManager");
            ViewBag.TotalOrders = await _db.Orders.CountAsync();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _db.UserAuthentications.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            // Set headers to prevent back-button access after logout
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult AccessDenied() => View();
        public IActionResult AdminInventory() => View();
        public IActionResult Report() => View();
        public IActionResult Stores() => View();
    }
}