using Microsoft.AspNetCore.Mvc;
using PackagedFoodFrontend.Models;

namespace PackagedFoodFrontend.Controllers
{
    public class UserController : Controller
    {
        private static Wallet _wallet = new Wallet { Balance = 0 };

        public IActionResult MyWallet()

        {

            return View(_wallet);

        }

        [HttpPost]

        public IActionResult AddMoney(decimal amount)

        {

            _wallet.Balance += amount;

            return RedirectToAction("MyWallet");

        }


        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult MyBasket()
        {
            return View();
        }
        public IActionResult MyOrders()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Logout() => RedirectToAction("Index", "Home");

        public IActionResult EditProfile()
        {
            return View();
        }
        public IActionResult DeliveryAddress()
        {
            return View();
        }
        public IActionResult EmailAddress()
        {
            return View();
        }
        public IActionResult SmartBasket()
        {
            return View();
        }
        public IActionResult PastOrders()
        {
            return View();
        }
        public IActionResult GiftCards()
        {
            return View();
        }

        public IActionResult AddAddress()

        {

            return View();

        }
        public IActionResult Payment()
        {
            return View();
        }


        // Handle Add Address form submission (POST)

        [HttpPost]

        public IActionResult AddAddress(string street, string city, string pin)

        {

            // For now, just simulate saving the address

            TempData["Message"] = $"New address added: {street}, {city}, {pin}";

            return RedirectToAction("DeliveryAddress");

        }
        public IActionResult AddGiftCard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGiftCard(string cardNumber, decimal amount, string expiry)
        {
            // For now, just simulate saving the card
            TempData["Message"] = $"Gift card added: {cardNumber} ₹{amount} – Expires: {expiry}";
            return RedirectToAction("GiftCards");
        }
        // GET: User/EditAddress/5
        public IActionResult EditAddress(int id)
        {
            // In a real app, you would fetch the address from a database using the 'id'
            // For now, we return the view
            return View();
        }

        [HttpPost]
        public IActionResult UpdateAddress(int id, string street, string city, string pin)
        {
            // Logic to save changes goes here
            TempData["Message"] = "Address updated successfully!";
            return RedirectToAction("DeliveryAddress");
        }

    }
}
