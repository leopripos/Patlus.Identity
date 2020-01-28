using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupDistributedCache
    {
        public static void ConfigureDistributedCacheService(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvirontment)
        {
            services.AddDistributedMemoryCache();
        }

        public static void ConfigureDistributedCache(this IApplicationBuilder app, IWebHostEnvironment env)
        { }
    }
}
