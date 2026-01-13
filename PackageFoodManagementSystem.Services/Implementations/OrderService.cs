using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using PackageFoodManagementSystem.Services.Interfaces;

using System;

using System.Collections.Generic;

namespace PackageFoodManagementSystem.Services.Implementations

{

    public class OrderService : IOrderService

    {

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)

        {

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

        public void PlaceOrder(Order order)

        {

            order.OrderDate = DateTime.Now;

            order.OrderStatus = "Pending";

            _orderRepository.AddOrder(order);

            _orderRepository.Save();

        }

        public void UpdateOrderStatus(int orderId, string status)

        {

            var order = _orderRepository.GetOrderById(orderId);

            if (order == null) return;

            order.OrderStatus = status;

            _orderRepository.UpdateOrder(order);

            _orderRepository.Save();

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
