using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.Persistence
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IMasterDbContext, MasterDbContext>(opt =>
            {
                opt.UseSqlServer(
                    configuration["Database:Connection"],
                    optBuilder => optBuilder.MigrationsAssembly("Patlus.IdentityManagement.Persistence")
                );
            });

            return services;
        }
    }
}
