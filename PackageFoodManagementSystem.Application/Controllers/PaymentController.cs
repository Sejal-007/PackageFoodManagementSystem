using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;

using System;
using System.Collections.Generic;
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
            ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]

        public IActionResult Confirm(int orderId)

        {

            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)

            {

                return BadRequest("Invalid Order. Order does not exist.");

            }

            var payment = new Payment

            {

                OrderID = orderId,

                PaymentMethod = "COD",

                PaymentStatus = "Success",

                PaymentDate = DateTime.Now,

                TransactionReference = Guid.NewGuid().ToString()

            };

            _context.Payments.Add(payment);

            order.OrderStatus = "Confirmed";

            _context.SaveChanges();

            return RedirectToAction("Success");

        }



    }
}