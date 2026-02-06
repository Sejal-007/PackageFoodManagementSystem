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

            int userId = GetUserId();

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

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            // 1. Create the Order

            var orderId = _orderService.CreateOrder(userId, deliveryAddress);

            // 2. Create the Bill immediately so Payment page finds it

            var existingBill = _context.Bill.FirstOrDefault(b => b.OrderID == orderId);

            if (existingBill == null)

            {

                // Calculate total from Cart/OrderItems

                var subtotal = _context.OrderItems

                    .Where(oi => oi.OrderID == orderId)

                    .Sum(oi => oi.Quantity * oi.UnitPrice);

                var newBill = new Bill

                {

                    OrderID = orderId,

                    BillDate = DateTime.Now,

                    SubtotalAmount = subtotal,

                    TaxAmount = 0,

                    DiscountAmount = 0,

                    FinalAmount = subtotal, // Matches your DB column 'FinalAmount'

                    BillingStatus = "Unpaid"

                };

                _context.Bill.Add(newBill);

                _context.SaveChanges();

            }

            // 3. Redirect to Payment

            return RedirectToAction("Payment", "Payment", new { orderId = orderId });

        }



        public IActionResult Success()
        {
            return View();
        }

        // Helper method to get the logged-in User ID
        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}