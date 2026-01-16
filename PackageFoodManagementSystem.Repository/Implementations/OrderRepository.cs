using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;

using PackageFoodManagementSystem.Repository.Models;

using System.Collections.Generic;

using System.Linq;

namespace PackageFoodManagementSystem.Repository.Implementations

{

    public class OrderRepository : IOrderRepository

    {

        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)

        {

            _context = context;

        }

        public IEnumerable<Order> GetAllOrders()

        {

            return _context.Orders.ToList();

        }

        public Order GetOrderById(int orderId)

        {

            return _context.Orders.FirstOrDefault(o => o.OrderId == orderId);

        }

        public void AddOrder(Order order)

        {

            _context.Orders.Add(order);

        }

        public void UpdateOrder(Order order)

        {

            _context.Orders.Update(order);

        }

        public void DeleteOrder(int orderId)

        {

            var order = _context.Orders.Find(orderId);

            if (order != null)

            {

                _context.Orders.Remove(order);

            }

        }

        public void Save()

        {

            _context.SaveChanges();

        }

    }

}
