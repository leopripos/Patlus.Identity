using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.IdentityManagement.Presentation;
using System.IO;

namespace Patlus.IdentityManagement.EventHandler
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .ValidateAutoMapper()
                .MigrateDatabase<Program>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.UseEnvironment(Environments.Development);
            hostBuilder.UseContentRoot(Directory.GetCurrentDirectory());

            var startup = new Startup();

            hostBuilder.ConfigureAppConfiguration((hostContext, builder) =>
            {
                startup.ConfigureAppConfiguration(hostContext, builder);
                builder.AddJsonFile("appsettings.json");
                builder.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                builder.AddEnvironmentVariables();
                builder.Build();
            });

            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                startup.ConfigureServices(hostContext, services);
                services.AddLogging();
            });

            return hostBuilder;
        }
    }
}
