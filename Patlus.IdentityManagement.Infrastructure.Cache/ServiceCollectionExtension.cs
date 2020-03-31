using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.Presentation.Security;
using Patlus.IdentityManagement.Infrastructure.Cache.Token;

namespace Patlus.IdentityManagement.Infrastructure.Cache
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            return services;
        }

        public static IServiceCollection AddTokenStorageCache(this IServiceCollection services)
        {
            services.AddSingleton<ITokenStorageService, DistributedTokenStorageService>();

            return services;
        }
    }
}
