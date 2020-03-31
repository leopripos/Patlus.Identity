using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Patlus.Common.Presentation.Services;
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
            builder.UseEnvironment("Development");
            builder.ConfigureServices(async services =>
            {
                // Replace Event Dispatcher
                var eventDispatcherService = services.SingleOrDefault(d => d.ServiceType == typeof(IEventDispatcher));
                if (eventDispatcherService != null) {
                    services.Remove(eventDispatcherService);
                    services.AddSingleton<IEventDispatcher, DummyEventDispatcher>();
                }
                
                // Replace Db Context
                var dbContextOptions = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MasterDbContext>));
                var masterDbInterface = services.SingleOrDefault(d => d.ServiceType == typeof(IMasterDbContext));
                if (dbContextOptions != null && masterDbInterface != null)
                {
                    services.Remove(dbContextOptions);
                    services.Remove(masterDbInterface);

                    var connection = $"IntegrationTesting{Guid.NewGuid()}.db";
                    services.AddDbContext<IMasterDbContext, MasterTestDbContext>(opt =>
                    {
                        opt.UseInMemoryDatabase(connection);
                    });
                }

                // Remake database with its data
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = (scopedServices.GetRequiredService<IMasterDbContext>() as MasterTestDbContext)!;
                var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                // Ensure the database is deleted and then created.
                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();
            });
        }
    }
}
