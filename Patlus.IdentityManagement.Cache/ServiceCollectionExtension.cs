using Microsoft.Extensions.DependencyInjection;
using Patlus.IdentityManagement.Cache.Services;

namespace Patlus.IdentityManagement.Cache
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            return services;
        }

        public static IServiceCollection AddTokenCacheService(this IServiceCollection services)
        {
            services.AddSingleton<ITokenStorageService, TokenDistributedCacheService>();

            return services;
        }
    }
}
