using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class CustomerServiceTests
    {
        private Mock<ICustomerRepository> _repoMock;
        private CustomerService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<ICustomerRepository>();
            _service = new CustomerService(_repoMock.Object);
        }

        [Test]
        public async Task GetAllAsync_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>());
            var res = await _service.GetAllAsync();
            Assert.IsNotNull(res);
        }

        [Test]
        public async Task GetByIdAsync_Delegates()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Customer { CustomerId = 1 });
            var res = await _service.GetByIdAsync(1);
            Assert.AreEqual(1, res.CustomerId);
        }

        [Test]
        public async Task AddUpdateDelete_Delegates()
        {
            var c = new Customer { Name = "T" };
            _repoMock.Setup(r => r.AddAsync(c)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateAsync(c)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.DeleteAsync(5)).Returns(Task.CompletedTask);

            await _service.AddAsync(c);
            await _service.UpdateAsync(c);
            await _service.DeleteAsync(5);

            _repoMock.Verify(r => r.AddAsync(c), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(c), Times.Once);
            _repoMock.Verify(r => r.DeleteAsync(5), Times.Once);
        }
    }
}
