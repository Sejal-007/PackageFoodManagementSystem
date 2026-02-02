using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Application.DTOs;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Security.Claims;

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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult MyBasket()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var cart = _context.Carts
          .Include(c => c.CartItems)
          .ThenInclude(ci => ci.Product)
          .FirstOrDefault(c => c.UserAuthenticationId == userId && c.IsActive);

        if (cart == null)
        {
            cart = new Cart
            {
                UserAuthenticationId = userId,
                CartItems = new List<CartItem>()
            };
        }

        return View("MyBasket", cart);
    }

    [HttpPost("Add")]
    public IActionResult Add([FromQuery] int productId) // Added [FromQuery]
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        _cartService.AddItem(userId, productId);
        return Ok();
    }

    [HttpPost("Decrease")]
    public IActionResult Decrease([FromQuery] int productId) // Added [FromQuery]
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        _cartService.DecreaseItem(userId, productId);
        return Ok();
    }

    [HttpGet("GetItemQty")]
    public IActionResult GetItemQty(int productId)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var cart = _context.Carts
          .Include(c => c.CartItems)
          .FirstOrDefault(c => c.UserAuthenticationId == userId && c.IsActive);

        if (cart == null) return Json(0);
        var item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        return Json(item?.Quantity ?? 0);
    }

    [HttpPost("Remove")]
    public IActionResult Remove([FromBody] CartRequest request)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        _cartService.Remove(userId, request.ProductId);
        return Ok();
    }

    [HttpGet("GetTotalItems")]
    public IActionResult GetTotalItems()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return Json(0);

        int userId = int.Parse(claim.Value);
        var totalCount = _context.CartItems
            .Where(ci => ci.Cart.UserAuthenticationId == userId && ci.Cart.IsActive)
            .Sum(ci => ci.Quantity);

        return Json(totalCount);
    }
}