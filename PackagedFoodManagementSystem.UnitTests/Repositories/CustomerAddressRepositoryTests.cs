using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using PackageFoodManagementSystem.Repository.Data;

using PackageFoodManagementSystem.Repository.Implementations;

using PackageFoodManagementSystem.Repository.Models;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

namespace PackageFoodManagementSystem.Tests

{

    [TestFixture]

    public class CustomerAddressRepositoryTests

    {

        private ApplicationDbContext _context;

        private CustomerAddressRepository _repository;

        [SetUp]

        public void Setup()

        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()

                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

                .Options;

            _context = new ApplicationDbContext(options);

            _repository = new CustomerAddressRepository(_context);

        }

        [TearDown]

        public void TearDown()

        {

            _context.Database.EnsureDeleted();

            _context.Dispose();

        }

        private CustomerAddress CreateValidAddress(int id, int customerId)

        {

            return new CustomerAddress

            {

                AddressId = id,

                CustomerId = customerId,

                AddressType = "Home",

                StreetAddress = "123 Main St",

                Landmark = "Near Park",

                City = "Chennai",

                PostalCode = "600001",

                Customer = new Customer

                {

                    CustomerId = customerId,

                    Name = "User",

                    Email = $"test{id}@test.com",

                    Phone = "1234567890"

                }

            };

        }

        [Test]

        public async Task GetAllAsync_ReturnsAddressesWithCustomerData()

        {

            // Arrange

            _context.CustomerAddresses.Add(CreateValidAddress(1, 10));

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetAllAsync();

            // Assert

            Assert.That(result.Count(), Is.EqualTo(1));

            Assert.That(result.First().Customer, Is.Not.Null);

        }

        [Test]

        public async Task GetByIdAsync_ReturnsCorrectAddress()

        {

            // Arrange

            var address = CreateValidAddress(5, 20);

            _context.CustomerAddresses.Add(address);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetByIdAsync(5);

            // Assert

            Assert.That(result, Is.Not.Null);

            Assert.That(result.AddressId, Is.EqualTo(5));

        }

        [Test]

        public async Task AddAsync_PersistsNewAddress()

        {

            // Arrange

            var address = CreateValidAddress(1, 10);

            // Act

            await _repository.AddAsync(address);

            // Assert

            var saved = await _context.CustomerAddresses.FindAsync(1);

            Assert.That(saved, Is.Not.Null);

            Assert.That(saved.StreetAddress, Is.EqualTo("123 Main St"));

        }

        [Test]

        public async Task DeleteAsync_RemovesRecordFromDatabase()

        {

            // Arrange

            var address = CreateValidAddress(1, 10);

            _context.CustomerAddresses.Add(address);

            await _context.SaveChangesAsync();

            // Act

            await _repository.DeleteAsync(1);

            // Assert

            var result = await _context.CustomerAddresses.FindAsync(1);

            Assert.That(result, Is.Null);

        }

    }

}
