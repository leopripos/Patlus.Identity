using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.RestTests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(async services =>
            {
                var dbContextOptions = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MasterDbContext>));
                if (dbContextOptions != null)
                {
                    services.Remove(dbContextOptions);
                }

                var masterDbInterface = services.SingleOrDefault(d => d.ServiceType == typeof(IMasterDbContext));
                if (masterDbInterface != null)
                {
                    services.Remove(masterDbInterface);
                }

                var connection = $"IntegrationTesting{Guid.NewGuid()}.db";
                services.AddDbContext<IMasterDbContext, MasterTestDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase(connection);
                });

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

#pragma warning disable IDE0063 // Use simple 'using' statement, Justification: I dont know this cause testing error if using simple `using`
                using (var scope = serviceProvider.CreateScope())
#pragma warning restore IDE0063 // Use simple 'using' statement
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IMasterDbContext>() as MasterTestDbContext;
                    var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    await db.Database.EnsureDeletedAsync();
                    await db.Database.EnsureCreatedAsync();
                }
            });
        }
    }
}
