using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Patlus.IdentityManagement.Presentation;

namespace Patlus.IdentityManagement.Rest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().ApplyPreRunSetup<Program>().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
