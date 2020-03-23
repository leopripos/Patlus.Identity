using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Patlus.IdentityManagement.UseCase.Services;
using System;

namespace Patlus.IdentityManagement.Presentation
{
    public static class HostExtension
    {
        public static IHost ApplyDatabaseMigration<TProgram>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<IMasterDbContext>() as DbContext;
                    db!.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<TProgram>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return host;
        }
    }
}
