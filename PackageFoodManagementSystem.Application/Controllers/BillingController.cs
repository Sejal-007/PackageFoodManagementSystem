using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;
namespace PackageFoodManagementSystem.Application.Controllers

{

    public class BillingController : Controller

    {

        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)

        {

            _billingService = billingService;

        }

        public IActionResult Generate(int orderId)

        {

            _billingService.GenerateBill(orderId);

            return RedirectToAction("Index", "Order");

        }

    }

}
