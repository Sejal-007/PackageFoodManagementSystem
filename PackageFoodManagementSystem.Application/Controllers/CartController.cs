using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Application.DTOs;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Security.Claims;

namespace PackageFoodManagementSystem.Controllers
{
    [Authorize]
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CartController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // ================== CART PAGE ==================
        [HttpGet("MyBasket")]
        public IActionResult MyBasket()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>(),
                    IsActive = true
                };
            }

            // Set ViewBag so the Layout can render the initial badge count
            ViewBag.CartCount = cart.CartItems.Sum(ci => ci.Quantity);

            return View("MyBasket", cart);
        }

        // ================== ADD / INCREASE ==================
        [HttpPost("Add")]
        public IActionResult Add([FromBody] CartRequest request)
        {
            // Defensive check to prevent NullReferenceException
            if (request == null || request.ProductId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data received." });
            }

            // Safely parse user ID
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            _cartService.AddItem(userId, request.ProductId);

            // Refresh cart data for response
            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            int newItemQty = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId)?.Quantity ?? 0;
            int totalCartCount = cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;

            return Json(new { success = true, newQty = newItemQty, cartCount = totalCartCount });
        }

        // ================== DECREASE ==================
        [HttpPost("Decrease")]
        public IActionResult Decrease([FromBody] CartRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _cartService.DecreaseItem(userId, request.ProductId);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            int newItemQty = cart?.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId)?.Quantity ?? 0;
            int totalCartCount = cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;

            return Json(new { success = true, newQty = newItemQty, cartCount = totalCartCount });
        }

        // ================== GET ITEM QUANTITY ==================
        [HttpGet("GetItemQty")]
        public IActionResult GetItemQty(int productId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            if (cart == null)
                return Json(0);

            var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
            return Json(item?.Quantity ?? 0);
        }

        // ================== REMOVE ==================
        [HttpPost("Remove")]
        public IActionResult Remove([FromBody] CartRequest request)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _cartService.Remove(userId, request.ProductId);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            int totalCartCount = cart?.CartItems.Sum(ci => ci.Quantity) ?? 0;

            return Json(new { success = true, cartCount = totalCartCount });
        }
    }
}