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
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

            StringContent requestContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var response = await _client.PostAsJsonAsync("Account/Login", requestContent);
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async void CreateProductTest()
        {
            // Arrange
            await AuthenticateAsAdmin();
            ProductViewModel product = new ProductViewModel();
            product.Name = "test";
            product.Description = "produit de test";
            product.Stock = "125";
            product.Price = "9999";
            product.Details = "details du produits test";


            var json = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            //var data = new StringContent(json,Encoding.UTF8, "application/json");



            // Act

            var response = await _client.PostAsJsonAsync("Product/Create", json);
            var allProducts =await _client.GetStringAsync("Product/Index");
            // Assert
            Assert.Contains("test", allProducts);
            
        }
        [Fact]
        public async void DeleteProductTest()
        {
            var client = _factory.CreateClient();
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}