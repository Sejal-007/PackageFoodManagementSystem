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








using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.AspNetCore.Http;

namespace PackagedFoodFrontend.Controllers
{
    public class UserController : Controller
    {
        private static Wallet _wallet = new Wallet { Balance = 0 };

        private UserAuthentication GetUserFromSession()
        {
            return new UserAuthentication
            {
                Name = HttpContext.Session.GetString("UserName") ?? "Guest",
                Email = HttpContext.Session.GetString("UserEmail") ?? "",
                MobileNumber = HttpContext.Session.GetString("UserMobile") ?? ""
            };
        }

        // Use this if your URL is /User/MyBasket
        //public IActionResult MyBasket()
        //{
        //    return View(GetUserFromSession());
        //}

        public IActionResult SmartBasket() => View(GetUserFromSession());
        public IActionResult Dashboard() => View(GetUserFromSession());
        public IActionResult EditProfile() => View(GetUserFromSession());
        public IActionResult EmailAddress() => View(GetUserFromSession());

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
            _wallet.Balance += amount;
            return RedirectToAction("MyWallet");
        }


        // 1. This opens the "Add" page
        [HttpGet]
        public IActionResult AddGiftCard()
        {
            return View();
        }

        // 2. This handles the "Activate" button click
        [HttpPost]
        public IActionResult AddGiftCard(string cardNumber, decimal amount, string expiry)
        {
            // Logic to save to database goes here
            TempData["Message"] = "Gift Card Activated Successfully!";
            return RedirectToAction("GiftCards");
        }

        public IActionResult GiftCards()
        {
            return View();
        }

        public IActionResult DeliveryAddress() => View();
        public IActionResult AddAddress() => View();
        public IActionResult MyOrders() => View();
        public IActionResult PastOrders() => View();
        public IActionResult Payment() => View();

        // Controllers/UserController.cs

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }






    }
}