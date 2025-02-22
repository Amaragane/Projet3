using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
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
}
