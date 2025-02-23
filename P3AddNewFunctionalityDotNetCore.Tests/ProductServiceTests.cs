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
            var content = new LoginModel
            {
                Name = "Admin",
                Password = "P@ssword123",
                ReturnUrl = "/"
            };
            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("Name", "Admin"),
            new KeyValuePair<string, string>("Password", "P@ssword123")
            };
            var data = new FormUrlEncodedContent(formData);
            var response = await _client.PostAsync("Account/Login", data);
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async void CreateProductTest()
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
        public async void DeleteProductTest()
        {

            
            await AuthenticateAsAdmin();

            var allProducts = await _client.GetStringAsync("Product/Index");
            Assert.Contains("id=\"1\"", allProducts);
            var formData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("id", "1")
            };
            var data = new FormUrlEncodedContent(formData);
            await _client.PostAsync("Product/DeleteProduct",data);
            allProducts = await _client.GetStringAsync("Product/Index");
            Assert.DoesNotContain("id=\"1\"", allProducts);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}