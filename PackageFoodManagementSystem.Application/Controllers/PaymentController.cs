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
    var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
    if (order == null) return BadRequest("Invalid Order.");

    var bill = _context.Bills.FirstOrDefault(b => b.OrderID == orderId);
    if (bill == null) return BadRequest("Bill not found.");

    string paymentStatus = (paymentMethod == "COD") ? "Pending" : "Success";
    string orderStatus = (paymentMethod == "COD") ? "Placed" : "Confirmed";

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

    _context.Payments.Add(payment);
    
    // Update statuses
    order.OrderStatus = orderStatus;
    if (paymentStatus == "Success")
    {
        bill.BillingStatus = "Paid";
    }

    _context.SaveChanges(); 

    return RedirectToAction("Success");
}

        public IActionResult Success()

        {

            return View();

        }

    }

}
