using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Controllers;
namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CustomeUnitsTestsFixture : WebApplicationFactory<Program>
    {

    }
    public class ProductServiceTests
    {
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void ExampleMethod()
        {
            // Arrange
            ProductViewModel product = new ProductViewModel();
            product.Id = 42;
            product.Name = "test";
            product.Description = "produit de test";
            product.Stock = "125";
            product.Price = "9999";
            product.Details = "details du produits test";

            // Act


            // Assert
            Assert.Equal(1, 1);
        }

        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}