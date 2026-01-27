using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
using System.Security.Claims;


namespace PackageFoodManagementSystem.Application.Controllers

{
    [Authorize]
    public class OrderController : Controller

    {

        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        public OrderController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public IActionResult Index()

        {

            var orders = _orderService.GetAllOrders();

            return View(orders);

        }

        [HttpPost]

        public IActionResult Create(Order order)

        {

            _orderService.PlaceOrder(order);

            return RedirectToAction("Index");

        }

        [HttpGet]

        public IActionResult Checkout()

        {

            if (!User.Identity.IsAuthenticated)

            {

                return RedirectToAction("Login", "Account");

            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var cart = _context.Carts

                .Include(c => c.CartItems)

                .ThenInclude(ci => ci.Product)

                .FirstOrDefault(c => c.UserAuthenticationId == userId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())

            {

                return RedirectToAction("MyBasket", "Cart");

            }

            return View(cart);

        }


        [HttpPost]
        public IActionResult PlaceOrder(string deliveryAddress)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orderId = _orderService.CreateOrder(userId, deliveryAddress);

            return RedirectToAction("Payment", "Payment", new { orderId });
        }

        public IActionResult Success()
        {
            return View();
        }

    }

}
