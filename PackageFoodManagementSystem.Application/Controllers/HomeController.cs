
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

        public IActionResult AdminInventory() => View();

        public IActionResult ManagerDashboard() => View();

        // HomeController.cs
        public IActionResult Users() => View();
        public IActionResult AboutUs() => View();

        public IActionResult ContactUs() => View();


    }
}
