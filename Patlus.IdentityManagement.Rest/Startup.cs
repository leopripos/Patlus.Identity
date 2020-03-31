using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.Common.Rest;
using Patlus.IdentityManagement.Presentation;
using Patlus.IdentityManagement.Rest.Extensions;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.Rest.StartupExtensions;
using System.Text.Json;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Patlus.IdentityManagement.Rest
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    options.AddDefaultExceptionFilters();
                    options.AddDynamicJsonCaseFormatter();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.AddDefaultOptions();
                });

            services.AddScoped<ValidPoolFilter>();

            services.AddAutoMapper(config => {
                config.AddIntegrationDispatcherMappings();
            }, GetType().Assembly);

            services.AddPresentationCore(_configuration);

            services.ConfigureCorsService();

            services.ConfigureAuthenticationService(_configuration);

            services.ConfigureAuthroizationService();

            if (_hostEnvironment.IsDevelopment())
            {
                services.ConfigureSwaggerService();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.ConfigureCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.ConfigureAuthentication();

            app.ConfigureAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (_hostEnvironment.IsDevelopment())
            {
                app.UseExceptionHandler("/development-error");
                app.ConfigureSwagger();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
        }
    }
}
