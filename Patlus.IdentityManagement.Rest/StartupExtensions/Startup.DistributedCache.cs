using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupDistributedCache
    {
        public static void ConfigureDistributedCacheService(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }
    }
}
