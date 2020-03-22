using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.IdentityManagement.Cache;
using Patlus.IdentityManagement.Persistence;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.Presentation;
using System.IO;

namespace Patlus.IdentityManagement.EventHandler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment(Environments.Development)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                       .AddJsonFile("appsettings.json")
                       .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                       .AddEnvironmentVariables()
                       .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDatabase(hostContext.Configuration);
                    services.AddUseCaseFeatures();
                    services.AddCacheService();
                    services.AddMachineService();
                });
    }
}
