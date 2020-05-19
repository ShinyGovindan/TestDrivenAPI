using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestDrivenAPI.Controllers;
using TestDrivenAPI.Models;

namespace TestWebApi
{
    [TestClass]
    public class TestProductBusinessRulesController
    {
        [TestMethod]
        public void GetAllProducts_ReturnAllProducts()
        {
            var products = GetPhysicalProducts();
            var controller = new ProductBusinessRulesController(products);

            var result = controller.GetAllProducts() as List<Product>;
            Assert.AreEqual(products.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnPhysicalProducts()
        {
            var testProducts = GetPhysicalProducts();
            var controller = new ProductBusinessRulesController(testProducts);

            var result = await controller.GetAllProductsAsync() as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
        }

        [TestMethod]
        public async Task GetAllNonPhysicalProductsAsync_ShouldReturnNonPhysicalProducts()
        {
            var testProducts = GetNonPhysicalProducts();
            var controller = new ProductBusinessRulesController(testProducts);

            var result = await controller.GetAllNonPhysicalProductsAsync() as List<Product>;
            Assert.AreEqual(testProducts.Count, result.Count);
        }

        private List<Product> GetPhysicalProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product { Id = 1, Name = "Book", Code="BK", Price = 3.75M , IsPhysicalProduct = true});
            testProducts.Add(new Product { Id = 2, Name = "Mobile", Code = "MB", Price = 100.00M, IsPhysicalProduct = true });
            testProducts.Add(new Product { Id = 3, Name = "Mouse", Code = "MS", Price = 850.00M, IsPhysicalProduct = true });
            testProducts.Add(new Product { Id = 4, Name = "Laptop", Code = "LP",Price = 55000.00M, IsPhysicalProduct = true });

            return testProducts;
        }

        private List<Product> GetNonPhysicalProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product { Id = 1, Name = "MemberShip", Code = "MB", Price = 3.75M , IsPhysicalProduct  = false});
            testProducts.Add(new Product { Id = 2, Name = "Prime", Code = "PM", Price = 100.00M , IsPhysicalProduct  = false});

            return testProducts;
        }
    }
}
