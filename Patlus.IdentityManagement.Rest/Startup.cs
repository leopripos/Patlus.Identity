using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Patlus.Common.Presentation.Responses.Content;
using Patlus.Common.Rest.Filters.Actions;
using Patlus.Common.Rest.Filters.Exceptions;
using Patlus.Common.Rest.Formatter.Json;
using Patlus.IdentityManagement.Presentation;
using Patlus.IdentityManagement.Rest.Extensions;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.Rest.StartupExtensions;
using System.Linq;
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
                    options.RespectBrowserAcceptHeader = true;

                    options.Filters.Add<AcceptCaseHeaderActionFilter>();
                    options.Filters.Add<NotFoundExceptionFilter>();
                    options.Filters.Add<ValidationExceptionFilter>();

                    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                    options.OutputFormatters.Add(new DynamicCaseJsonOutputFormater());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var jsonOptions = context.HttpContext.RequestServices.GetRequiredService<IOptionsSnapshot<JsonOptions>>();

                        var errors = context.ModelState.ToDictionary(
                            item => item.Key,
                            item => item.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                        return new BadRequestObjectResult(
                            new ValidationErrorDto(errors)
                        );
                    };
                });

            services.AddScoped<ValidPoolFilter>();

            services.AddAutoMapper(GetType().Assembly, typeof(NotificationDispatcherModule).Assembly);

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
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger();
            }
        }
    }
}
