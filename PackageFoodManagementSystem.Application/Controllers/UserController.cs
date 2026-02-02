using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PackagedFoodFrontend.Controllers
{
    public class UserController : Controller
    {
        private readonly ICustomerAddressService _addressService;
        private readonly ApplicationDbContext _context;

        public UserController(ICustomerAddressService addressService, ApplicationDbContext context)
        {
            _addressService = addressService;
            _context = context;
        }

        #region Dashboard & Profile

        public async Task<IActionResult> Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("SignIn", "Home");

            // Meta Data
            ViewBag.FullName = HttpContext.Session.GetString("UserName");
            ViewBag.Email = HttpContext.Session.GetString("UserEmail");
            ViewBag.Phone = HttpContext.Session.GetString("UserPhone");

            // FIXED: Comparing int to int directly (No .ToString() needed)
            ViewBag.TotalOrders = await _context.Orders
                .CountAsync(o => o.CreatedByUserID == userId.Value);

            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId);

            ViewBag.WalletBalance = userWallet?.Balance ?? 0m;

            return View();
        }

        public IActionResult EditProfile() => View(GetUserFromSession());

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserAuthentication model)
        {
            HttpContext.Session.SetString("UserName", model.Name ?? "Guest");
            HttpContext.Session.SetString("UserPhone", model.MobileNumber ?? "");
            // In a real app, call: await _userService.UpdateProfileAsync(model);
            return Json(new { success = true });
        }

        public IActionResult MyBasket() => View(GetUserFromSession());
        public IActionResult SmartBasket() => View(GetUserFromSession());
        public async Task<IActionResult> MyOrders()
        {
            // 1. Get the logged-in User ID from Session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("SignIn", "Home");

            // 2. Fetch orders from DB where CreatedByUserID matches
            var orders = await _context.Orders
            .Where(o => o.CustomerId == userId.Value || o.CreatedByUserID == userId.Value)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

            // 3. Pass the list to the view
            return View(orders);
        }

        [HttpPost]
        public IActionResult CancelOrder(int orderId)
        {
            try
            {
                // Use the OrderService to update the status in the backend
                var orderService = (IOrderService)HttpContext.RequestServices.GetService(typeof(IOrderService));
                orderService.CancelOrder(orderId); // This updates DB to "Cancelled"

                return Json(new { success = true, message = "Order cancelled successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult PastOrders() => View();

        public IActionResult Payment() => View();
        public IActionResult GiftCards() => View();

        [HttpGet]
        public IActionResult AddGiftCard() => View();

        [HttpPost]
        public IActionResult AddGiftCard(string cardNumber, decimal amount, string expiry)
        {
            TempData["Message"] = "Gift Card Activated Successfully!";
            return RedirectToAction("GiftCards");
        }



        #endregion

        #region Wallet & Payments

        public async Task<IActionResult> MyWallet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                wallet = new Wallet { UserId = userId.Value, Balance = 0 };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            return View(wallet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMoney(decimal amount)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || amount <= 0) return RedirectToAction("MyWallet");

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet != null)
            {
                wallet.Balance += amount;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyWallet");
        }

        #endregion

        #region Address Management

        public async Task<IActionResult> DeliveryAddress()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("SignIn", "Home");

            var allAddresses = await _addressService.GetAllAsync();
            var userAddresses = allAddresses.Where(x => x.CustomerId == userId).ToList();

            return View(userAddresses);
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("SignIn", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(CustomerAddress address)
        {
            // 1. Get the real CustomerId from the session
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("SignIn", "Home");
            }

            // 2. Assign the ID before validation
            address.CustomerId = userId.Value;

            // 3. Remove validation for fields the user doesn't fill manually
            ModelState.Remove("CustomerId");
            ModelState.Remove("Customer");

            if (ModelState.IsValid)
            {
                try
                {
                    // 4. Save to DB
                    await _addressService.AddAsync(address);

                    TempData["Message"] = "Address saved successfully!";

                    // 5. Success Redirect
                    return RedirectToAction("DeliveryAddress");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save to database. Error: " + ex.Message);
                }
            }

            // If we reach here, something failed; reload the form with errors
            return View(address);
        }


        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("SignIn", "Home");

            await _addressService.DeleteAsync(id);
            TempData["Message"] = "Address removed!";
            return RedirectToAction("DeliveryAddress");
        }

        #endregion

        #region Helpers & Auth

        private UserAuthentication GetUserFromSession()
        {
            return new UserAuthentication
            {
                Id = HttpContext.Session.GetInt32("UserId") ?? 0,
                Name = HttpContext.Session.GetString("UserName") ?? "Guest",
                Email = HttpContext.Session.GetString("UserEmail") ?? "",
                MobileNumber = HttpContext.Session.GetString("UserPhone") ?? ""
            };
        }

        public IActionResult EmailAddress() => View(GetUserFromSession());
        public IActionResult ContactUs() => View();
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Welcome", "Home");
        }

        #endregion
    }
}