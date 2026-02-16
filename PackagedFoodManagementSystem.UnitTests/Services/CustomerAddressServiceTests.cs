using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using PackageFoodManagementSystem.Repository.Data;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestFixture]
public class CustomerAddressServiceTests
{
    private ApplicationDbContext _context;
    private Mock<ICustomerAddressRepository> _mockRepo;
    private CustomerAddressService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomerAddressDB_" + System.Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _mockRepo = new Mock<ICustomerAddressRepository>();
        _service = new CustomerAddressService(_context, _mockRepo.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
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
    public async Task GetByIdAsync_CallsRepository()
    {
        // Arrange
        var address = new CustomerAddress { AddressId = 5 };
        _mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(address);

        // Act
        var result = await _service.GetByIdAsync(5);

        // Assert
        Assert.That(result, Is.EqualTo(address));
        _mockRepo.Verify(r => r.GetByIdAsync(5), Times.Once);
    }
}