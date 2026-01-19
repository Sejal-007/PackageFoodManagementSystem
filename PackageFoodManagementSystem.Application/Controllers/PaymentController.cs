//using Microsoft.AspNetCore.Mvc;
//using PackageFoodManagementSystem.Repository.Models;
//using PackageFoodManagementSystem.Services.Interfaces;
//namespace PackagedFoodFrontend.Controllers // Updated namespace to match Solution Explorer

//{

//    public class PaymentController : Controller

//    {

//        [HttpGet]

//        public IActionResult Checkout()

//        {

//            return View(); // Refers to Views/Payment/Checkout.cshtml

//        }

//        [HttpPost]

//        public IActionResult Confirm(string method)

//        {

//            TempData["PaymentMethod"] = method;

//            return RedirectToAction("Success");

//        }

//        public IActionResult Success()

//        {

//            return View(); // Refers to Views/Payment/Success.cshtml

//        }

//    }

//}









using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PackagedFoodFrontend.Controllers
{
    public class PaymentController : Controller
    {
        // Simulated database for orders
        private static List<OrderModel> _orderHistory = new List<OrderModel>();

        [HttpGet]
        public IActionResult Checkout() => View();

        [HttpPost]
        public IActionResult Confirm(string method, string amount)
        {
            if (string.IsNullOrEmpty(method))
            {
                ModelState.AddModelError("", "Please select a payment method.");
                return View("Checkout");
            }

            // 1. Create the order based on login and current payment
            var newOrder = new OrderModel
            {
                OrderId = "#" + new Random().Next(10000, 99999),
                UserName = User.Identity.Name ?? "yayati", // Uses logged-in name
                Amount = amount ?? "0.00",
                Method = method,
                Date = DateTime.Now,
                Status = "Delivered"
            };

            // 2. Save order to history
            _orderHistory.Add(newOrder);

            // 3. Pass data to Success page
            TempData["PaymentMethod"] = method;
            TempData["OrderTotal"] = amount;

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            if (TempData["PaymentMethod"] == null)
            {
                return RedirectToAction("Checkout");
            }
            return View();
        }

        [HttpGet]
        public IActionResult PastOrders()
        {
            var currentUser = User.Identity.Name ?? "yayati";
            // Filter list to show only current user's orders
            var userOrders = _orderHistory
                .Where(o => o.UserName == currentUser)
                .OrderByDescending(o => o.Date)
                .ToList();

            return View(userOrders);
        }
    }

    // Simple model for the order
    public class OrderModel
    {
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public string Amount { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}