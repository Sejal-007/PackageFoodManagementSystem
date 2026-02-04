using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Implementations;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Models;
using System.Linq;

namespace PackagedFoodManagementSystem.UnitTests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private ApplicationDbContext _context;
        private ProductRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ProductRepoDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repo = new ProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddAndGetProduct_Works()
        {
            var p = new Product { ProductName = "X", Price = 10, Category = "C", Quantity = 1 };
            _repo.AddProduct(p);
            _repo.Save();

            var all = _repo.GetAllProducts().ToList();
            Assert.AreEqual(1, all.Count);
            var got = _repo.GetProductById(p.ProductId);
            Assert.IsNotNull(got);
            Assert.AreEqual("X", got.ProductName);
        }
    }
}
