using Microsoft.AspNetCore.Mvc;

using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Interfaces;


namespace PackageFoodManagementSystem.Application.Controllers

{

    public class OrderController : Controller

    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)

        {

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

    }

}
