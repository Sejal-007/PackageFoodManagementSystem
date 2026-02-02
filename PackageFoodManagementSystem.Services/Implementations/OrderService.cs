using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using System;

using System.Collections.Generic;

namespace PackageFoodManagementSystem.Services.Implementations

{

    public class OrderService : IOrderService

    {
        private readonly ApplicationDbContext _context;

        private readonly IOrderRepository _orderRepository;

        public OrderService(ApplicationDbContext context ,IOrderRepository orderRepository)

        {
            _context = context;
            _orderRepository = orderRepository;

        }

        public IEnumerable<Order> GetAllOrders()

        {

            return _orderRepository.GetAllOrders();

        }

        public Order GetOrderById(int id)

        {

            return _orderRepository.GetOrderById(id);

        }

        public int CreateOrder(int userId, string address)

        {

            var cart = _context.Carts

                .Include(c => c.CartItems)

                .ThenInclude(ci => ci.Product)

                .FirstOrDefault(c => c.UserAuthenticationId == userId && c.IsActive);

            if (cart == null || !cart.CartItems.Any())

                throw new Exception("Cart is empty");

            var order = new Order
            {
                CustomerId = userId,
                CreatedByUserID = userId, // ADD THIS LINE: It was missing!
                DeliveryAddress = address,
                OrderStatus = "PendingPayment",
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart.CartItems)

            {

                _context.OrderItems.Add(new OrderItem

                {

                    OrderID = order.OrderID,

                    ProductId = item.ProductId,

                    Quantity = item.Quantity,

                    UnitPrice = item.Product.Price,

                    Subtotal = item.Quantity * item.Product.Price

                });

            }

            order.TotalAmount = cart.CartItems.Sum(x => x.Quantity * x.Product.Price);

            cart.IsActive = false; // 🔒 lock cart

            _context.SaveChanges();

            return order.OrderID;

        }


        public void PlaceOrder(Order order)

        {

            order.OrderDate = DateTime.Now;

            order.OrderStatus = "Pending";

            _orderRepository.AddOrder(order);

            _orderRepository.Save();

        }

        // Change 'string changedBy' to 'int changedByUserId'
        public void UpdateOrderStatus(int orderId, string status, string changedBy, string remarks)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null) return;

            order.OrderStatus = status;
            order.LastUpdateOn = DateTime.Now;

            var history = new OrderStatusHistory
            {
                OrderID = orderId,
                Status = status,
                ChangedOn = DateTime.Now,
                ChangedBy = 1, // Use a number here because your DB column is an INT
                Remarks = remarks
            };

            _context.OrderStatusHistories.Add(history);
            _context.SaveChanges();
        }

        public void CancelOrder(int orderId)

        {

            var order = _orderRepository.GetOrderById(orderId);

            if (order == null) return;

            if (order.OrderStatus == "Preparing" || order.OrderStatus == "Dispatched")

            {

                throw new Exception("Order cannot be cancelled after preparation has started.");

            }

            order.OrderStatus = "Cancelled";

            _orderRepository.UpdateOrder(order);

            _orderRepository.Save();



        }

        
    }

}
