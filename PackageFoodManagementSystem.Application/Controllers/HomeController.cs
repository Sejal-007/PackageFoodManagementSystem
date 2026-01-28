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

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config; // for JWT

        public HomeController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index() => View();

        public IActionResult Welcome()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        // --- SIGN IN (Cookie-based, unchanged behaviour) ---
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
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPhone", user.MobileNumber ?? "");

                if (user.Role == "Admin") return RedirectToAction("AdminDashboard");
                if (user.Role == "StoreManager") return RedirectToAction("Home", "StoreManager");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        // --- JWT SignIn for API/Mobile ---
        [HttpPost]
        [Route("api/signin")]
        [AllowAnonymous]
        public async Task<IActionResult> ApiSignIn(string email, string password)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
            {
                var token = JwtHelper.GenerateJwtToken(user, _config);
                return Ok(new { Token = token, UserId = user.Id, Name = user.Name, Email = user.Email, Role = user.Role });
            }

            return Unauthorized("Invalid email or password.");
        }

        // --- SIGN UP ---
        [HttpGet]
        public IActionResult SignUp() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserAuthentication user)
        {
            if (!ModelState.IsValid) return View(user);

            try
            {
                var userId = await _userService.CreateUserAsync(
                    user.Name,
                    user.MobileNumber,
                    user.Email,
                    user.Password);

                TempData["SuccessMessage"] = "Account created! Please sign in.";
                return RedirectToAction(nameof(SignIn));
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.InnerException?.Message ?? ex.Message}";
                return View(user);
            }
        }

        // --- DASHBOARDS ---
        [Authorize(Roles = "User")]
        public IActionResult Dashboard() => View();

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalCustomers = await _userService.CountUsersByRoleAsync("User");
            ViewBag.TotalStoreManagers = await _userService.CountUsersByRoleAsync("StoreManager");
            // Orders can be moved to OrderService similarly
            return View();
        }


        // --- USER MANAGEMENT (CRUD) ---
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserAuthentication user)
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _userService.AddUserAsync(user);
            TempData["SuccessMessage"] = "User added successfully!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserAuthentication user)
        {
            await _userService.UpdateUserAsync(user);
            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction(nameof(Users));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            // Prevent caching of authenticated pages
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("SignIn", "Home");
        }


    }
}
