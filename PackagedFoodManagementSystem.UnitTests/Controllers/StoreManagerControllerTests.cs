using NUnit.Framework;
using PackageFoodManagementSystem.Application.Controllers;
using PackageFoodManagementSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PackagedFoodManagementSystem.UnitTests.Controllers
{
    [TestFixture]
    public class StoreManagerControllerTests
    {
        private StoreManagerController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Set up In-Memory Database for the Controller
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "StoreManagerTestDb_" + Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Pass the context to the constructor
            _controller = new StoreManagerController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Home_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Home());

        [Test]
        public void Profile_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.Profile());

        [Test]
        public void Orders_ReturnsView()
        {
            // FIX: Passed null/empty string because the controller method 'Orders(string status)' requires it
            var res = _controller.Orders(null);
            Assert.IsInstanceOf<ViewResult>(res);
        }

        [Test]
        public void OrdersDashboard_ReturnsView() => Assert.IsInstanceOf<ViewResult>(_controller.OrdersDashboard());

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