using Moq;
using NUnit.Framework;
using PackageFoodManagementSystem.Services.Implementations;
using PackageFoodManagementSystem.Repository.Interfaces;
using PackageFoodManagementSystem.Repository.Models;
using System;

namespace PackagedFoodManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class BillingServiceTests
    {
        private Mock<IBillRepository> _billRepo;
        private Mock<IPaymentRepository> _paymentRepo;
        private BillingService _service;

        [SetUp]
        public void Setup()
        {
            _billRepo = new Mock<IBillRepository>();
            _paymentRepo = new Mock<IPaymentRepository>();
            _service = new BillingService(_billRepo.Object, _paymentRepo.Object);
        }

        [Test]
        public void GenerateBill_CreatesBillAndSaves()
        {
            _billRepo.Setup(r => r.AddBill(It.IsAny<Bill>()));
            _billRepo.Setup(r => r.Save());

            var bill = _service.GenerateBill(10);
            Assert.AreEqual(10, bill.OrderID);
            Assert.AreEqual("Generated", bill.BillingStatus);
            _billRepo.Verify(r => r.AddBill(It.IsAny<Bill>()), Times.Once);
            _billRepo.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void MakePayment_SetsStatusAndSaves()
        {
            var p = new Payment { OrderID = 1, BillID = 1, PaymentMethod = "Cash", PaymentStatus = "Pending", TransactionReference = "T1" };
            _paymentRepo.Setup(r => r.AddPayment(It.IsAny<Payment>()));
            _paymentRepo.Setup(r => r.Save());

            _service.MakePayment(p);
            Assert.AreEqual("Success", p.PaymentStatus);
            Assert.IsNotNull(p.PaymentDate);
            _paymentRepo.Verify(r => r.AddPayment(It.IsAny<Payment>()), Times.Once);
            _paymentRepo.Verify(r => r.Save(), Times.Once);
        }
    }
}
