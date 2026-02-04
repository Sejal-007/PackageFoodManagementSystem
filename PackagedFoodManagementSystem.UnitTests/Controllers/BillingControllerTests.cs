using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class BillingControllerTests
    {
        private Mock<IBillingService> _billingMock;
        private BillingController _controller;

        [SetUp]
        public void Setup()
        {
            _billingMock = new Mock<IBillingService>();
            _controller = new BillingController(_billingMock.Object);
        }

        [Test]
        public void Generate_RedirectsToOrderIndex()
        {
            var res = _controller.Generate(5);
            Assert.IsInstanceOf<RedirectToActionResult>(res);
            _billingMock.Verify(b => b.GenerateBill(5), Times.Once);
        }
    }
}
