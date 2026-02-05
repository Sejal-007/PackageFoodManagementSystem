using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Application.Controllers
{
    public class MenuController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        // Updated constructor to include ICartService
        public MenuController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        

        // GET: Menu/Index
        public async Task<IActionResult> Index(string category)
        {
            // FIX: Use _productService (matching the variable defined above)
            var products = await _productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category).ToList();
            }

            // FIX: Get current user ID and fetch the active cart
            var userId = GetUserId();
            var cart = await _cartService.GetActiveCartAsync(userId);

            // Map cart items to a dictionary for quick lookup in the view
            var cartItems = cart?.CartItems?.ToDictionary(i => i.ProductId, i => i.Quantity)
                            ?? new Dictionary<int, int>();

            ViewBag.CartItems = cartItems;
            ViewBag.SelectedCategory = category;

            return View(products);
        }

        // GET: Menu/Details/5
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Helper method to get the logged-in User ID
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out int id) ? id : 0;
        }
    }
}