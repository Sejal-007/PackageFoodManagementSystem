using Microsoft.AspNetCore.Mvc;

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
