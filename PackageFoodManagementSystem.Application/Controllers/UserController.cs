//using Microsoft.AspNetCore.Mvc;
//using PackageFoodManagementSystem.Repository.Models;

//namespace PackagedFoodFrontend.Controllers
//{
//    public class UserController : Controller
//    {
//        private static Wallet _wallet = new Wallet { Balance = 0 };

//        public IActionResult MyWallet()

//        {

//            return View(_wallet);

//        }

//        [HttpPost]

//        public IActionResult AddMoney(decimal amount)

//        {

//            _wallet.Balance += amount;

//            return RedirectToAction("MyWallet");

//        }


//        public IActionResult Dashboard()
//        {
//            return View();
//        }
//        public IActionResult MyBasket()
//        {
//            return View();
//        }
//        public IActionResult MyOrders()
//        {
//            return View();
//        }
//        public IActionResult ContactUs()
//        {
//            return View();
//        }
//        public IActionResult Logout() => RedirectToAction("Index", "Home");

//        public IActionResult EditProfile()
//        {
//            return View();
//        }
//        public IActionResult DeliveryAddress()
//        {
//            return View();
//        }
//        public IActionResult EmailAddress()
//        {
//            return View();
//        }
//        public IActionResult SmartBasket()
//        {
//            return View();
//        }
//        public IActionResult PastOrders()
//        {
//            return View();
//        }
//        public IActionResult GiftCards()
//        {
//            return View();
//        }

//        public IActionResult AddAddress()

//        {

//            return View();

//        }
//        public IActionResult Payment()
//        {
//            return View();
//        }


//        // Handle Add Address form submission (POST)

//        [HttpPost]

//        public IActionResult AddAddress(string street, string city, string pin)

//        {

//            // For now, just simulate saving the address

//            TempData["Message"] = $"New address added: {street}, {city}, {pin}";

//            return RedirectToAction("DeliveryAddress");

//        }
//        public IActionResult AddGiftCard()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult AddGiftCard(string cardNumber, decimal amount, string expiry)
//        {
//            // For now, just simulate saving the card
//            TempData["Message"] = $"Gift card added: {cardNumber} ₹{amount} – Expires: {expiry}";
//            return RedirectToAction("GiftCards");
//        }
//        // GET: User/EditAddress/5
//        public IActionResult EditAddress(int id)
//        {
//            // In a real app, you would fetch the address from a database using the 'id'
//            // For now, we return the view
//            return View();
//        }

//        [HttpPost]
//        public IActionResult UpdateAddress(int id, string street, string city, string pin)
//        {
//            // Logic to save changes goes here
//            TempData["Message"] = "Address updated successfully!";
//            return RedirectToAction("DeliveryAddress");
//        }

//    }
//}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Threading.Tasks;

namespace PackagedFoodFrontend.Controllers
{
    public class UserController : Controller
    {
        private readonly ICustomerAddressService _addressService;
        private static Wallet _wallet = new Wallet { Balance = 0 };

        public UserController(ICustomerAddressService addressService)
        {
            _addressService = addressService;
        }

        private UserAuthentication GetUserFromSession()
        {
            return new UserAuthentication
            {
                Name = HttpContext.Session.GetString("UserName") ?? "Guest",
                Email = HttpContext.Session.GetString("UserEmail") ?? "",
                MobileNumber = HttpContext.Session.GetString("UserMobile") ?? ""
            };
        }

        public IActionResult Dashboard() => View(GetUserFromSession());
        public IActionResult MyBasket() => View(GetUserFromSession());
        public IActionResult SmartBasket() => View(GetUserFromSession());
        public IActionResult EditProfile() => View(GetUserFromSession());
        public IActionResult EmailAddress() => View(GetUserFromSession());
        public IActionResult MyOrders() => View();
        public IActionResult PastOrders() => View();
        public IActionResult Payment() => View();
        public IActionResult ContactUs() => View();
        public IActionResult GiftCards() => View();

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult MyWallet()
        {
            ViewBag.WalletBalance = _wallet.Balance;
            return View(_wallet);
        }

        [HttpPost]
        public IActionResult AddMoney(decimal amount)
        {
            if (amount > 0)
            {
                _wallet.Balance += amount;
                TempData["Message"] = $"Added ₹{amount} to wallet!";
            }
            return RedirectToAction("MyWallet");
        }

        [HttpGet]
        public IActionResult AddGiftCard() => View();

        [HttpPost]
        public IActionResult AddGiftCard(string cardNumber, decimal amount, string expiry)
        {
            TempData["Message"] = "Gift Card Activated Successfully!";
            return RedirectToAction("GiftCards");
        }

        // --- Delivery Address Management ---

        public async Task<IActionResult> DeliveryAddress()
        {
            // Fetches all addresses to display in the list
            var addresses = await _addressService.GetAllAsync();
            return View(addresses);
        }

        [HttpGet]
        public IActionResult AddAddress() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(CustomerAddress address)
        {
            if (ModelState.IsValid)
            {
                address.CustomerId = HttpContext.Session.GetInt32("UserId") ?? 1;
                await _addressService.AddAsync(address);
                return RedirectToAction("DeliveryAddress");
            }
            return RedirectToAction("DeliveryAddress");
        }

        // Action specifically linked to your Modal "Save Address" button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAddress(CustomerAddress address)
        {
            // Fixes the 404 error by providing a valid endpoint
            if (ModelState.IsValid)
            {
                address.CustomerId = HttpContext.Session.GetInt32("UserId") ?? 1;

                // Saves the new Landmark and AddressType fields
                await _addressService.AddAsync(address);
            }

            // Refreshes the page to show the newly added address
            return RedirectToAction("DeliveryAddress");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserAuthentication model)
        {
            // 1. Call your Service/Repository to update the SQL Database
            // await _userService.UpdateProfileAsync(model); 

            // 2. Update the session so the change shows in the Navbar immediately
            HttpContext.Session.SetString("UserName", model.Name);
            HttpContext.Session.SetString("UserMobile", model.MobileNumber);

            return Json(new { success = true });
        }
    }
}