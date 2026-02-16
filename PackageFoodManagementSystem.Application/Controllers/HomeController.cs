using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.DTOs;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Helpers;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public HomeController(
            IUserService userService,
            IOrderService orderService,
            IInventoryService inventoryService,
            IProductService productService,
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _userService = userService;
            _orderService = orderService;
            _inventoryService = inventoryService;
            _productService = productService;
            _config = config;
        }

        [Authorize(Roles = "User")]
        public IActionResult Index() => View();

        public IActionResult Welcome()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index");
            return View();
        }

        /// <summary>
        /// Orders Dashboard: KPI counts + optionally filtered list
        /// </summary>
        public async Task<IActionResult> OrdersDashboard(string status)
        {
            // 1) KPI counts
            ViewBag.TodayOrders = await _orderService.CountTodayOrdersAsync();
            ViewBag.PendingOrders = await _orderService.CountPendingOrdersAsync();
            ViewBag.CompletedOrders = await _orderService.CountCompletedOrdersAsync();
            ViewBag.CancelledOrders = await _orderService.CountCancelledOrdersAsync();

            // 2) Query list (includes Customer)
            var orders = await _orderService.GetOrdersAsync(status);

            // 3) Never return null
            return View(orders ?? new List<Order>());
        }

        /// <summary>
        /// Legacy Orders page: compute tiles and redirect to Dashboard w/ status
        /// </summary>
        public async Task<IActionResult> Orders(string status)
        {
            // 1) KPI tiles BEFORE filtering (kept same semantics)
            ViewBag.TodayOrders = await _orderService.CountTodayOrdersAsync();
            ViewBag.PendingOrders = await _orderService.CountPendingOrdersAsync();
            ViewBag.CompletedOrders = await _orderService.CountCompletedOrdersAsync();
            ViewBag.CancelledOrders = await _orderService.CountCancelledOrdersAsync();

            // Original action ultimately redirects to OrdersDashboard
            return RedirectToAction("OrdersDashboard", new { status });
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

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserPhone", user.MobileNumber ?? "");

                if (user.Role == "Admin") return RedirectToAction("AdminDashboard");
                if (user.Role == "StoreManager") return RedirectToAction("Home", "StoreManager");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            TempData["ErrorMessage"] = "Invalid Credentials";
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
                await _userService.CreateUserAsync(user.Name, user.MobileNumber, user.Email, user.Password, user.Role);
                TempData["SuccessMessage"] = "Account created! Please sign in.";
                return RedirectToAction(nameof(SignIn));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(user);
            }
        }

        /// <summary>
        /// Admin dashboard KPIs (users, orders) and extra metrics using services only.
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            // Base stats from user service (existing)
            var (totalCustomers, totalStoreManagers, totalOrders) = await _userService.GetAdminDashboardStatsAsync();
            ViewBag.TotalCustomers = totalCustomers;
            ViewBag.TotalStoreManagers = totalStoreManagers;
            ViewBag.TotalOrders = totalOrders;

            // Extra metrics from order/product services
            ViewBag.TotalRevenue = await _orderService.SumTotalRevenueAsync();
            ViewBag.RecentOrders = await _orderService.GetRecentOrdersAsync(5); // includes Customer

            var products = (await _productService.GetAllProductsAsync())?.ToList() ?? new List<Product>();
            ViewBag.LowStockCount = products.Count(p => p.Quantity <= 10);

            // If you want TotalBatches via service, expose from IBatchService or InventoryService
            // For now, derive via inventory list (BatchId distinct)
            var inventory = (await _inventoryService.GetInventoryListAsync())?.ToList() ?? new List<Inventory>();
            ViewBag.TotalBatches = inventory.Select(x => x.BatchId).Distinct().Count();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        /// <summary>
        /// All orders with Customer, latest first.
        /// </summary>
        public async Task<IActionResult> OrderStatus()
        {
            var orders = await _orderService.GetAllOrdersWithCustomerAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("SignIn", "Home");
        }

        /// <summary>
        /// Processes order status (including FIFO stock reduction when Delivered).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(int orderId, string status)
        {
            await _orderService.ProcessOrderAsync(orderId, status);
            return RedirectToAction("OrdersDashboard");
        }

        /// <summary>
        /// Admin report using service aggregation (same semantics).
        /// </summary>
        public async Task<IActionResult> Report()
        {
            var reportData = await _orderService.BuildAdminReportAsync();
            return View(reportData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserAuthentication user)
        {
            if (!ModelState.IsValid)
                return View("Users", await _userService.GetAllUsersAsync());

            await _userService.CreateUserAsync(user.Name, user.MobileNumber, user.Email, user.Password, user.Role);
            TempData["SuccessMessage"] = "User added successfully!";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserAuthentication user)
        {
            var res = await _userService.UpdateUserAsync(user);
            if (!res.Success)
            {
                TempData["ErrorMessage"] = res.ErrorMessage;
                return View("Users", await _userService.GetAllUsersAsync());
            }

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminInventory()
        {
            var inventoryData = (await _inventoryService.GetInventoryListAsync())?.ToList() ?? new List<Inventory>();
            var products = (await _productService.GetAllProductsAsync())?.ToList() ?? new List<Product>();

            var categoryList = products
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            ViewBag.TotalCategories = categoryList.Count;
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalBatches = inventoryData.Select(x => x.BatchId).Distinct().Count();
            ViewBag.OutOfStock = products.Count(p => p.Quantity <= 0);
            ViewBag.LowStock = products.Count(p => p.Quantity <= 20);
            ViewBag.CategoryList = categoryList;
            ViewBag.AllProductsList = products;
            ViewBag.OutOfStockList = products.Where(p => p.Quantity <= 0).ToList();
            ViewBag.LowStockList = products.Where(p => p.Quantity <= 20).ToList();

            return View(inventoryData);
        }

        public IActionResult AboutUs() => View();
        public IActionResult ContactUs() => View();
        public IActionResult AccessDenied() => View();
        public IActionResult Stores() => View();
    }
}
