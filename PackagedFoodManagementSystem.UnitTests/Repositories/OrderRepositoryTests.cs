using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private ApplicationDbContext _context;
        private OrderRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("OrderRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new OrderRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddAndGetOrder_Works()
        {
            var o = new Order { CreatedByUserID = 1, CustomerId = 1, OrderStatus = "Pending", TotalAmount = 0 };
            _repo.AddOrder(o);
            _repo.Save();

            var all = _repo.GetAllOrders().ToList();
            Assert.AreEqual(1, all.Count);
        }
    }
}
