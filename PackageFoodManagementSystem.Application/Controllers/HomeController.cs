
using Microsoft.AspNetCore.Mvc;

namespace PackagedFoodManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult SignIn() => View();

        public IActionResult SignUp() => View();

        public IActionResult Dashboard() => View();

        public IActionResult AdminDashboard() => View();


        public IActionResult ManagerDashboard() => View();

        public IActionResult Users() => View();

        public IActionResult AboutUs() => View();

        public IActionResult ContactUs() => View();

        public IActionResult AdminInventory()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult Stores()
        {
            return View();
        }
    }
}
