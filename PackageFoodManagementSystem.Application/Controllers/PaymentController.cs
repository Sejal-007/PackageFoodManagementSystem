using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IOrderService _orderService; // Add this line


        public PaymentController(ApplicationDbContext context, IOrderService orderService)
        {

            _context = context;

            _orderService = orderService; // Assign it here
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
            // 1. Validate Order exists
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null)
            {
                return BadRequest("Order not found");
            }

    var bill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);
    if (bill == null) return BadRequest("Bill not found.");
            // 2. Validate Bill exists (required for Payment record)
            var Bill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);
            if (bill == null)
            {
                return BadRequest("Bill not found for this order.");
            }

    string paymentStatus = (paymentMethod == "COD") ? "Pending" : "Success";
    string orderStatus = (paymentMethod == "COD") ? "Placed" : "Confirmed";
            // 3. Set Logic: COD stays 'Placed', Online Payments become 'Confirmed'
            string PaymentStatus = (paymentMethod == "COD") ? "Pending" : "Success";
            string nextOrderStatus = (paymentMethod == "COD") ? "Placed" : "Confirmed";

    // Create the payment record
    var payment = new Payment
    {
        BillID = bill.BillID,
        OrderID = orderId,
        PaymentMethod = paymentMethod,
        PaymentStatus = paymentStatus,
        PaymentDate = DateTime.Now,
        TransactionReference = Guid.NewGuid().ToString()
        // REMOVE 'AmountPaid' here if it is causing the error
    };
            // 4. Create and Save Payment Record
            var Payment = new Payment
            {
                BillID = bill.BillID,
                OrderID = orderId,
                PaymentMethod = paymentMethod,
                PaymentStatus = paymentStatus,
                PaymentDate = DateTime.Now,
                TransactionReference = Guid.NewGuid().ToString(),
                AmountPaid = bill.FinalAmount // Ensure you set the amount from the bill
            };

    _context.Payments.Add(payment);
    
    // Update statuses
    order.OrderStatus = orderStatus;
    if (paymentStatus == "Success")
    {
        bill.BillingStatus = "Paid";
    }
            _context.Payments.Add(payment);
            _context.SaveChanges(); // Save payment first

    _context.SaveChanges(); 
            // 5. AUTOMATIC STATUS TRANSITION
            // This calls your service logic to update the Order table 
            // AND create the entry in OrderStatusHistories automatically.
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

    }

}
