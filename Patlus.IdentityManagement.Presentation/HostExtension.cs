using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.Presentation
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TProgram>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetRequiredService<IMasterDbContext>() as DbContext;
                db!.Database.Migrate();
            }

            return host;
        }

        public static IHost ValidateAutoMapper(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var mapper = services.GetRequiredService<IMapper>();
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }

            return host;
        }
    }
}
