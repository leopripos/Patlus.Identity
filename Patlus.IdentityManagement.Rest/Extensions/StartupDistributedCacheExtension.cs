using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupDistributedCacheExtension
    {
        public static void ConfigureDistributedCacheService(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvirontment)
        {
            services.AddDistributedMemoryCache();
        }
    }
}
