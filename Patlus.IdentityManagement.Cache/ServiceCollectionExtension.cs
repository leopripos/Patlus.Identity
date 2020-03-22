using Microsoft.Extensions.DependencyInjection;
using Patlus.IdentityManagement.Cache.Services;

namespace Patlus.IdentityManagement.Cache
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSingleton<ITokenCacheService, TokenDistributedCacheService>();

            return services;
        }
    }
}
