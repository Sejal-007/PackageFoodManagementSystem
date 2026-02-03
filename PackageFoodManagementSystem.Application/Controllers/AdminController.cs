using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackageFoodManagementSystem.Application.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;

        // Constructor to inject the service
        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult ProcessOrder(int orderId, string status)
        {
            // Get Admin ID from logged-in user
            int adminId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            // Manual transition: Admin moves order to next phase
            _orderService.UpdateOrderStatus(orderId, status, adminId.ToString(), $"Admin updated status to {status}");

            return RedirectToAction("Dashboard");
        }
    }
}