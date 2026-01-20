

using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductService _service;

        public MenuController(IProductService service) => _service = service;

        public IActionResult Index()
        {
            // This pulls the same data the manager just saved
            var products = _service.GetAllProducts();
            return View(products);
        }
    }
}
