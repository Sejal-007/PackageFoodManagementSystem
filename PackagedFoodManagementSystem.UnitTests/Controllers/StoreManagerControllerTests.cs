using NUnit.Framework;
using PackageFoodManagementSystem.Application.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class StoreManagerControllerTests
    {
        private StoreManagerController _controller;

        [SetUp]
        public void Setup() => _controller = new StoreManagerController();

        [Test]
        public void Home_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Home());

        [Test]
        public void Profile_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Profile());

        [Test]
        public void Orders_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Orders());

        [Test]
        public void AddProduct_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.AddProduct());

        [Test]
        public void Inventory_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Inventory());

        [Test]
        public void Reports_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Reports());

        [Test]
        public void Compliance_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Compliance());

        [Test]
        public void Settings_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Settings());
    }
}
