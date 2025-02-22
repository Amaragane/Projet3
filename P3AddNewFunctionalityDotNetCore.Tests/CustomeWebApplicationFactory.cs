using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CustomeTestsFixture<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
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
}
