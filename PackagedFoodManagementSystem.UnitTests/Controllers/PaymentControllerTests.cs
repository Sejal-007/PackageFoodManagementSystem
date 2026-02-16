using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Services.Interfaces;
using PackageFoodManagementSystem.Repository.Models;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _paymentMock;
        private PaymentController _controller;

        [SetUp]
        public void Setup()
        {
            _paymentMock = new Mock<IPaymentService>();
            _controller = new PaymentController(_paymentMock.Object);
        }

        [Test]
        public void Confirm_Post_Success_RedirectsToSuccess()
        {
            // Arrange
            int orderId = 101;
            // Fix CS1503: Return a Tuple (bool, string) instead of an anonymous object
            _paymentMock.Setup(s => s.ConfirmPayment(orderId, "Card", "1234"))
                        .Returns((true, string.Empty));

            // Act
            var result = _controller.Confirm(orderId, "Card", "1234");

            // Assert
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect?.ActionName, Is.EqualTo("Success"));
        }

        [Test]
        public void Confirm_Post_Failure_RedirectsToFailure()
        {
            // Arrange
            int orderId = 101;
            // Fix CS1503: Return a Tuple (bool, string)
            _paymentMock.Setup(s => s.ConfirmPayment(orderId, "Card", "0000"))
                        .Returns((false, "Insufficient Funds"));

            // Act
            var result = _controller.Confirm(orderId, "Card", "0000");

            // Assert
            var redirect = result as RedirectToActionResult;
            Assert.That(redirect?.ActionName, Is.EqualTo("Failure"));
            Assert.That(redirect?.RouteValues["orderId"], Is.EqualTo(orderId));
        }
    }
}