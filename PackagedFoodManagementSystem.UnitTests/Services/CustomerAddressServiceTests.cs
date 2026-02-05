using NUnit.Framework;
using Moq;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class CustomerAddressServiceTests
    {
        private ApplicationDbContext _context;
        private Mock<ICustomerAddressRepository> _repoMock;
        private CustomerAddressService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CA_TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _repoMock = new Mock<ICustomerAddressRepository>();
            _service = new CustomerAddressService(_context, _repoMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllAsync_ReturnsFromContext()
        {
            _context.CustomerAddresses.Add(new CustomerAddress { AddressId = 1, CustomerId = 2, AddressType = "Home", StreetAddress = "A", Landmark = "L1", City = "C", PostalCode = "00001" });
            _context.SaveChanges();

            var res = await _service.GetAllAsync();
            Assert.IsNotEmpty(res);
        }

        [Test]
        public async Task GetByIdAsync_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new CustomerAddress { AddressId = 5, CustomerId = 9, AddressType = "Home", StreetAddress = "S", Landmark = "L2", City = "C", PostalCode = "00002" });
            var res = await _service.GetByIdAsync(5);
            Assert.AreEqual(5, res.AddressId);
        }

        [Test]
        public async Task AddAsync_AddsAndSaves()
        {
            var addr = new CustomerAddress { AddressId = 10, CustomerId = 3, AddressType = "Home", StreetAddress = "X", Landmark = "L3", City = "C", PostalCode = "00003" };
            await _service.AddAsync(addr);
            var list = _context.CustomerAddresses.ToList();
            Assert.IsNotEmpty(list);
        }

        [Test]
        public async Task GetAddressesByUserIdAsync_Filters()
        {
            _context.CustomerAddresses.Add(new CustomerAddress { AddressId = 11, CustomerId = 8, AddressType = "Home", StreetAddress = "Z", Landmark = "L4", City = "C", PostalCode = "00004" });
            _context.SaveChanges();

            var res = await _service.GetAddressesByUserIdAsync(8);
            Assert.IsNotEmpty(res);
        }
    }
}
