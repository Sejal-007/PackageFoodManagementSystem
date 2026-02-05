using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private Mock<ICustomerService> _serviceMock;
        private CustomerController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ICustomerService>();
            _controller = new CustomerController(_serviceMock.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithCustomers()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Customer>());
            var res = await _controller.Index();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenCustomerNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((Customer)null);
            var res = await _controller.Details(5);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public async Task Details_ReturnsView_WhenCustomerFound()
        {
            var c = new Customer { CustomerId = 3, Name = "T" };
            _serviceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(c);
            var res = await _controller.Details(3);
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            var res = _controller.Create();
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            _controller.ModelState.AddModelError("x", "err");
            var res = await _controller.Create(new Customer());
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Create_Post_Valid_Redirects()
        {
            var cust = new Customer { CustomerId = 1, Name = "A" };
            var res = await _controller.Create(cust);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.AddAsync(cust), Times.Once);
        }

        [Test]
        public async Task Edit_Get_ReturnsNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(9)).ReturnsAsync((Customer)null);
            var res = await _controller.Edit(9);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public async Task Edit_Get_ReturnsView_WhenFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new Customer { CustomerId = 2 });
            var res = await _controller.Edit(2);
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Edit_Post_BadRequest_OnIdMismatch()
        {
            var customer = new Customer { CustomerId = 5 };
            var res = await _controller.Edit(6, customer);
            Assert.IsInstanceOf<BadRequestResult>(res);
        }

        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            var customer = new Customer { CustomerId = 7 };
            _controller.ModelState.AddModelError("x", "err");
            var res = await _controller.Edit(7, customer);
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public async Task Delete_Get_ReturnsNotFound_WhenNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync((Customer)null);
            var res = await _controller.Delete(10);
            Assert.IsInstanceOf<NotFoundResult>(res);
        }

        [Test]
        public async Task DeleteConfirmed_Redirects()
        {
            var res = await _controller.DeleteConfirmed(4);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _serviceMock.Verify(s => s.DeleteAsync(4), Times.Once);
        }
    }
}
