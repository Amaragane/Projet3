using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using Moq;
using Moq.Protected;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Controllers;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System.Runtime.InteropServices;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Net.Http;
using System.Net;
using System.Security.Principal;
namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CustomeTestsFixture : WebApplicationFactory<Program>
    {
        private const string ConnectionString = "Server=.;Database=UnitDbTest;Trusted_Connection=True;";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<P3Referential>));
                services.Remove(descriptor);
                var descriptorIdentity = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppIdentityDbContext>));
                services.Remove(descriptorIdentity);
                services.AddDbContext<P3Referential>(options => options.UseSqlServer(ConnectionString));
                services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer("Server=DESKTOP-LUCID6O;Database=Identity;Trusted_Connection=True"));

                using var scope = services.BuildServiceProvider().CreateScope();
                var scopeServices = scope.ServiceProvider;
                var dbContext = scopeServices.GetRequiredService<P3Referential>();
                using var scopeIdentity = services.BuildServiceProvider().CreateScope();
                var scopeServicesIdentity = scope.ServiceProvider;
                var dbContextIdentity = scopeServices.GetRequiredService<AppIdentityDbContext>();
                Utilities.Cleanup(dbContext);
                Utilities.SeedDatabase(dbContext);
                //Utilities.InitializeDatabase(dbContext);
            });
        }
            
    }
        public static class Utilities
        {
            public static void InitializeDatabase(P3Referential context)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedDatabase(context);

            }
            public static void Cleanup(P3Referential context)
            {
                var products = context.Product.ToList();
                context.Product.RemoveRange(products);
                var orders = context.Order.ToList();
                context.Order.RemoveRange(orders);
                var orderLines = context.OrderLine.ToList();
                context.OrderLine.RemoveRange(orderLines);

        }
            public static void SeedDatabase(P3Referential context)
            {
                if (context.Product.Any())
                {
                    return;
                }
                context.Product.AddRange(
                 new Product
                 {
                     Name = "Echo Dot",
                     Description = "(2nd Generation) - Black",
                     Quantity = 10,
                     Price = 92.50
                 },

                 new Product
                 {
                     Name = "Anker 3ft / 0.9m Nylon Braided",
                     Description = "Tangle-Free Micro USB Cable",
                     Quantity = 20,
                     Price = 9.99
                 },

                 new Product
                 {
                     Name = "JVC HAFX8R Headphone",
                     Description = "Riptidz, In-Ear",
                     Quantity = 30,
                     Price = 69.99
                 },

               new Product
               {
                   Name = "VTech CS6114 DECT 6.0",
                   Description = "Cordless Phone",
                   Quantity = 40,
                   Price = 32.50
               },

               new Product
               {
                   Name = "NOKIA OEM BL-5J",
                   Description = "Cell Phone",
                   Quantity = 50,
                   Price = 895.00
               }
            );
                context.SaveChanges();
            }
        }
    public class ProductServiceTests  : IClassFixture<CustomeTestsFixture>
    {
        public CustomeTestsFixture _factory;
        public ProductServiceTests(CustomeTestsFixture factory)
        {
            _factory = factory;
        }
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void IsAuthenticatedTest()
        {
             const string User = "Admin";
             const string Password = "P@ssword123";

        }
        [Fact]
        public async void CreateProductTest()
        {
            // Arrange
            var client = _factory.CreateClient();

            ProductViewModel product = new ProductViewModel();
            product.Name = "test";
            product.Description = "produit de test";
            product.Stock = "125";
            product.Price = "9999";
            product.Details = "details du produits test";


            var json = JsonSerializer.Serialize(product);
            var data = new StringContent(json,System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            LoginModel login = new LoginModel {Name = "Admin", Password = "P@ssword123" };
            var loginJson = JsonSerializer.Serialize(login);
            var dataLogin = new StringContent(loginJson, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            // Act
            var connect = await client.PostAsync("http://localhost:60700/Account/Login" , dataLogin);
            var response = await client.PostAsync("http://localhost:60700/Product/Create", data);
            // Assert
            Assert.Equal(HttpStatusCode.OK ,response.StatusCode);

            var responseContent = response.Content.ReadAsStringAsync();

            
            Console.WriteLine(response.Content);
            
        }
        [Fact]
        public async void DeleteProductTest()
        {
            var client = _factory.CreateClient();
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}