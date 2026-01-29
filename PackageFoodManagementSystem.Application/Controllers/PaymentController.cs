using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Models;

using System;

using System.Linq;

namespace PackagedFoodFrontend.Controllers

{

    [Authorize]

    public class PaymentController : Controller

    {

        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)

        {

            _context = context;

        }

        public IActionResult Payment(int orderId)

        {

            if (orderId == 0)
                return BadRequest("OrdeeId missing from request");

            ViewBag.OrderId = orderId;

            return View();

        }

        // ✅ Confirm Payment

        [HttpPost]

        public IActionResult Confirm(int orderId, string paymentMethod)

        {

            // 1️⃣ Validate Order

            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)

            {

                return BadRequest("Invalid Order. Order does not exist.");

            }

            // 2️⃣ Validate Bill

            var bill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);

            if (bill == null)

            {

                return BadRequest("Bill not found for this order.");

            }

            // 3️⃣ Decide Status based on Payment Method

            string paymentStatus;

            string orderStatus;

            if (paymentMethod == "COD")

            {

                paymentStatus = "Pending";   // Admin will confirm later

                orderStatus = "Placed";

            }

            else

            {

                paymentStatus = "Success";   // UPI / Card

                orderStatus = "Confirmed";

            }

            // 4️⃣ Create Payment (REQUIRED MEMBERS SET ✅)

            var payment = new Payment

            {

                BillID = bill.BillID,

                OrderID = orderId,

                PaymentMethod = paymentMethod,

                PaymentStatus = paymentStatus,   // 🔥 FIXED ERROR

                PaymentDate = DateTime.Now,

                TransactionReference = Guid.NewGuid().ToString()

            };

            _context.Payments.Add(payment);

            // 5️⃣ Update Order Status

            order.OrderStatus = orderStatus;

            _context.SaveChanges();

            return RedirectToAction("Success");

        }

        public IActionResult Success()

        {

            return View();

        }

    }

}
