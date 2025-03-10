using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

using System;
using System.Text.Json;
using System.Net.Http;
using System.Net;
using System.Security.Principal;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Xml.Linq;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
namespace P3AddNewFunctionalityDotNetCore.Tests
{
    
        
    public class ProductServiceTests  : IClassFixture<CustomeTestsFixture<Program>>
    {
        public CustomeTestsFixture<Program> _factory;
        public HttpClient _client;
        public ProductServiceTests(CustomeTestsFixture<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _client.BaseAddress=new Uri("http://localhost:60700/");
        }
        private async Task AuthenticateAsAdmin()
        {

            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("Name", "Admin"),
            new KeyValuePair<string, string>("Password", "P@ssword123")
            };
            var data = new FormUrlEncodedContent(formData);
            var response = await _client.PostAsync("Account/Login", data);
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public void CheckProductModelErrorsTest()
        {
            // Arrange

            var list = new ProductViewModel
            {
                Id = 1,
                Name = "test",
                Description = "test",
                Details = "test",
                Stock = "10",
                Price = "92,50"
            };
            var localizer = Mock.Of<IStringLocalizer<ProductService>>();
            var cart = Mock.Of<ICart>();
            var orderRepo = Mock.Of<IOrderRepository>();
            var productRepo = Mock.Of<IProductRepository>();
            var productService = new ProductService(cart,productRepo,orderRepo,localizer);

            // Act
            var products = productService.CheckProductModelErrors(list);
             
            // Assert
            Assert.Empty(products);
        }
        [Fact]
        public void GetAllProductsTest()
        {
            // Arrange

            var list = new List<Product>{new Product
            {
                Id = 1,
                Name = "test",
                Description = "test",
                Details = "test",
                Quantity = 10,
                Price = 92.50
            }
            };
            var localizer = Mock.Of<IStringLocalizer<ProductService>>();
            var cart = Mock.Of<ICart>();
            var orderRepo = Mock.Of<IOrderRepository>();
            var productRepo = Mock.Of<IProductRepository>();
            var productService = new ProductService(cart, productRepo, orderRepo, localizer);
            Mock.Get(productRepo).Setup(p => p.GetAllProducts())
                .Returns(list);
            // Act
            var products = productService.GetAllProducts();
            // Assert
            Assert.NotEmpty(products);
        }
        
        
        [Fact]
        public async Task DoNotAuthenticateAsAdminTest()
        {
            // Arrange
            
            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("Name", "Test"),
            new KeyValuePair<string, string>("Password", "Test")
            };
            var data = new FormUrlEncodedContent(formData);
            // Act
            await _client.PostAsync("Account/Logout", null);
            var response = await _client.PostAsync("Account/Login", data);
            var index = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Contains("Invalid name or password", index);

        }

        [Fact]
        public async Task CreateProductTest()
        {
            // Arrange
            
            await AuthenticateAsAdmin();
            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("Name", "test"),
            new KeyValuePair<string, string>("Description", "produit de test"),
            new KeyValuePair<string, string>("Stock", "125"),
            new KeyValuePair<string, string>("Price", "9999"),
            new KeyValuePair<string, string>("Details", "details du produits test")
            };

            var data = new FormUrlEncodedContent(formData);
            
            // Act

            var response = await _client.PostAsync("Product/Create", data);
            var allProducts =await _client.GetStringAsync("Product/Index");

            // Assert
            Assert.Contains("test", allProducts);
        }
        [Fact]
        public async Task DeleteProductTest()
        {

            // Arrange
            await AuthenticateAsAdmin();

            
            
            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("id", "1")
            };
            var data = new FormUrlEncodedContent(formData);
            // Act
            await _client.PostAsync("Product/DeleteProduct",data);

            var allProducts = await _client.GetStringAsync("Product/Admin");
            
            
            // Assert

            Assert.DoesNotContain("id=\"1\"", allProducts);
        }

  
    }
}