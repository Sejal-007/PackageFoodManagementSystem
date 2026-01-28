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


        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]

        public IActionResult PlaceOrder(string deliveryAddress)

        {

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 1️⃣ Create Order

            var orderId = _orderService.CreateOrder(userId, deliveryAddress);

            // 2️⃣ Generate Bill IMMEDIATELY

            var order = _context.Orders

                .Include(o => o.OrderItems)

                .ThenInclude(oi => oi.Product)

                .First(o => o.OrderID == orderId);

            decimal subtotal = order.OrderItems.Sum(x => x.Quantity * x.Product.Price);

            var bill = new Bill

            {

                OrderID = orderId,

                BillDate = DateTime.Now,

                SubtotalAmount = subtotal,

                TaxAmount = 0,

                DiscountAmount = 0,

                FinalAmount = subtotal,

                BillingStatus = "Generated"

            };

            _context.Bills.Add(bill);

            _context.SaveChanges();

            // 3️⃣ Redirect to Payment Page

            return RedirectToAction("Payment", "Payment", new { orderId });

        }


    }

}
