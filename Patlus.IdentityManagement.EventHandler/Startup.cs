using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.IdentityManagement.Presentation;
using System.Diagnostics.CodeAnalysis;

namespace Patlus.IdentityManagement.EventHandler
{
    public sealed class Startup
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "For basic template")]
        public void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder builder)
        {
        }

        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddPresentationCore(hostContext.Configuration);
        }
    }
}
