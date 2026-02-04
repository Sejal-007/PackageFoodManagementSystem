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
            try
            {
                int userId = GetUserId();

                // 1. Create the Order (This call works now because the Service no longer sends 'Subtotal')
                var orderId = _orderService.CreateOrder(userId, deliveryAddress);

                // 2. Create the Bill immediately so Payment page finds it
                var existingBill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);
                if (existingBill == null)
                {
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
                        FinalAmount = subtotal,
                        BillingStatus = "Unpaid"
                    };

                    _context.Bills.Add(newBill);
                    _context.SaveChanges();
                }

                // 3. Redirect to Payment
                return RedirectToAction("Payment", "Payment", new { orderId = orderId });
            }
            catch (Exception ex)
            {
                // Fixes the NullReferenceException in Error.cshtml
                return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
            }
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