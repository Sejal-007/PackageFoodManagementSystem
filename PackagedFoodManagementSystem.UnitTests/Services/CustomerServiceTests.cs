using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using PackageFoodManagementSystem.Services.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class CustomerServiceTests
{
    private Mock<ICustomerRepository> _mockRepo;
    private CustomerService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<ICustomerRepository>();
        _service = new CustomerService(_mockRepo.Object);
    }

    [Test]
    public async Task GetByUserIdAsync_FiltersCorrectly()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { CustomerId = 1, UserId = 101 },
            new Customer { CustomerId = 2, UserId = 102 }
        };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

        // Act
        var result = await _service.GetByUserIdAsync(102);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.CustomerId, Is.EqualTo(2));
    }

    [Test]
    public async Task AddAsync_CallsRepoOnce()
    {
        // Arrange
        var customer = new Customer { Name = "New Customer" };

        // Act
        await _service.AddAsync(customer);

        // Assert
        _mockRepo.Verify(r => r.AddAsync(customer), Times.Once);
    }
}