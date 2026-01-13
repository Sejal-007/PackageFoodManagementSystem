using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
namespace PackagedFoodFrontend.Controllers // Updated namespace to match Solution Explorer

{

    public class PaymentController : Controller

    {

        [HttpGet]

        public IActionResult Checkout()

        {

            return View(); // Refers to Views/Payment/Checkout.cshtml

        }

        [HttpPost]

        public IActionResult Confirm(string method)

        {

            TempData["PaymentMethod"] = method;

            return RedirectToAction("Success");

        }

        public IActionResult Success()

        {

            return View(); // Refers to Views/Payment/Success.cshtml

        }

    }

}
