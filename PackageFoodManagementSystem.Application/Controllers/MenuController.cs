

using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductService _service;

        public MenuController(IProductService service) => _service = service;

        public IActionResult Index(string? category)
        {
            // Fetch all products from the service
            var products = _service.GetAllProducts();

            // If a category is selected, filter the list
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category).ToList();
                ViewBag.SelectedCategory = category; // This helps the UI know what is selected
            }

            return View(products);
        }
    }
}
