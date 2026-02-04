using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class CartServiceTests
    {
        private ApplicationDbContext _context;
        private CartService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CartTestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _service = new CartService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddItem_CreatesCartAndItem()
        {
            _service.AddItem(1, 10);
            var cart = _service.GetActiveCart(1);
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.CartItems.Count);
        }

        [Test]
        public void DecreaseItem_RemovesWhenZero()
        {
            _service.AddItem(2, 20);
            _service.DecreaseItem(2, 20);
            var cart = _service.GetActiveCart(2);
            Assert.IsEmpty(cart.CartItems);
        }
    }
}
