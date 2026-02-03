using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Implementations;
using System;
using System.Linq;

namespace PackagedFoodFrontend.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public PaymentController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public IActionResult Payment(int orderId)
        {
            if (orderId == 0)
                return BadRequest("OrderId missing from request");

            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]
        public IActionResult Confirm(int orderId, string paymentMethod, string cardNumber)
        {
            // 1. Validate Order exists
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null) return BadRequest("Order not found");

            // 2. Validate Bill exists
            var bill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);
            if (bill == null) return BadRequest("Bill not found for this order.");

            // --- SIMULATED PAYMENT FAILURE LOGIC ---
            // If user enters 16 zeros, we treat it as a bank decline
            if (paymentMethod == "Card" && !string.IsNullOrEmpty(cardNumber))
            {
                string cleanCardNo = cardNumber.Replace(" ", "");
                if (cleanCardNo == "0000000000000000")
                {
                    // Update Order Status to reflect failure in history
                    _orderService.UpdateOrderStatus(orderId, "Payment Failed", "System", "Card declined by bank.");
                    return RedirectToAction("Failure", new { orderId = orderId });
                }
            }

            // 3. Set Logic for Successful Path
            string paymentStatus = (paymentMethod == "COD") ? "Pending" : "Success";
            string nextOrderStatus = (paymentMethod == "COD") ? "Placed" : "Confirmed";

            // 4. Create and Save Payment Record
            var paymentEntry = new Payment
            {
                BillID = bill.BillID,
                OrderID = orderId,
                PaymentMethod = paymentMethod,
                PaymentStatus = paymentStatus,
                PaymentDate = DateTime.Now,
                TransactionReference = Guid.NewGuid().ToString(),
                AmountPaid = bill.FinalAmount 
            };

            _context.Payments.Add(paymentEntry);

            // Update Billing Status if paid online
            if (paymentStatus == "Success")
            {
                bill.BillingStatus = "Paid";
            }

            _context.SaveChanges(); 

            // 5. Update Order Status & History via Service
            _orderService.UpdateOrderStatus(
                orderId,
                nextOrderStatus,
                "System_Auto_Payment",
                $"Payment processed via {paymentMethod}"
            );

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failure(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}