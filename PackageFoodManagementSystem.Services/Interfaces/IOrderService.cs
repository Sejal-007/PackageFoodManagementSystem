using PackageFoodManagementSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackageFoodManagementSystem.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int orderId);
        void PlaceOrder(Order order);
        void UpdateOrderStatus(int orderId, string status);
        void CancelOrder(int orderId);
        int CreateOrder(int userId, string deliveryAddress);
    }
}
