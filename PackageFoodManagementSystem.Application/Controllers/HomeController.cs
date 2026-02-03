using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PackageFoodManagementSystem.DTOs;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
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

        private readonly ApplicationDbContext _context;


        public HomeController(IUserService userService, IConfiguration config, ApplicationDbContext context, ApplicationDbContext db)
        {
            _userService = userService;
            _config = config;
            _context = context;
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

        [HttpPost]
        public IActionResult ProcessOrder(int orderId, string status)
        {
            // 1. Find the order in the database
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order != null)
            {
                // 2. Update the status based on which button was clicked
                order.OrderStatus = status;

                // 3. Save changes to the database
                _context.SaveChanges();
            }

            // 4. Redirect back to the dashboard to see the updated badge
            return RedirectToAction("OrdersDashboard");
        }

        public IActionResult Report()
        {
            var reportData = new AdminReportDto
            {
                // 1. Calculate Lifetime Revenue from all non-cancelled orders
                LifetimeRevenue = _context.Orders
                    .Where(o => o.OrderStatus != "Cancelled")
                    .Sum(o => o.TotalAmount),

                // 2. Count total registered customers
                TotalCustomers = _context.Customers.Count(),

                // 3. Count orders that are not yet Delivered or Cancelled
                TotalActiveOrders = _context.Orders
                    .Count(o => o.OrderStatus == "Confirmed" || o.OrderStatus == "Processing"),

                // 4. Join Orders and Customers to find big spenders
                TopCustomers = _context.Orders
                    .Where(o => o.OrderStatus != "Cancelled")
                    .GroupBy(o => o.CustomerId)
                    .Select(g => new TopCustomerDto
                    {
                        // We find the customer name/email using the ID from the group
                        Name = _context.Customers
                            .Where(c => c.CustomerId == g.Key)
                            .Select(c => c.Name)
                            .FirstOrDefault() ?? "Unknown",

                        Email = _context.Customers
                            .Where(c => c.CustomerId == g.Key)
                            .Select(c => c.Email)
                            .FirstOrDefault() ?? "N/A",

                        OrderCount = g.Count(),
                        TotalSpent = g.Sum(o => o.TotalAmount)
                    })
                    .OrderByDescending(x => x.TotalSpent)
                    .Take(5) // Just show the top 5
                    .ToList()
            };

            return View(reportData);
        }

        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult AccessDenied() => View();
        public IActionResult AdminInventory() => View();
        public IActionResult Stores() => View();
    }
}