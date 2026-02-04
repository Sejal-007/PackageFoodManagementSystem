using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using PackageFoodManagementSystem.Controllers;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class StoreControllerTests
    {
        private StoreController _controller;

        [SetUp]
        public void Setup() => _controller = new StoreController();

        [Test]
        public void AddProduct_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.AddProduct());

        [Test]
        public void Report_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Report());
    }
}
