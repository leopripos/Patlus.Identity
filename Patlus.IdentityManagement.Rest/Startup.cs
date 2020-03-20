using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.Rest.Extensions;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.Rest.Filters.Exceptions;
using Patlus.IdentityManagement.Rest.Formatter.Json;
using Patlus.IdentityManagement.Rest.Responses.Content;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.Rest.StartupExtensions;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
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

            services.AddMediatR(ModuleProfile.GetBundles());
            services.AddValidatorsFromAssemblies(ModuleProfile.GetBundles());
            services.AddAutoMapper(GetType().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<ITimeService, TimeService>();
            services.AddSingleton<ITokenCacheService, TokenDistributedCacheService>();
            services.AddSingleton<ITokenService, JwtBearerTokenService>();
            services.AddSingleton<IPasswordService, HMACSHA1PasswordService>();

            services.AddDbContext<IMasterDbContext, MasterDbContext>(opt =>
            {
                opt.UseSqlServer(
                    _configuration["Database:Connection"],
                    optBuilder => optBuilder.MigrationsAssembly("Patlus.IdentityManagement.Persistence")
                );
            });

            services.ConfigureCorsService(_configuration);

            services.ConfigureDistributedCacheService(_configuration, _hostEnvironment);

            services.ConfigureAuthenticationService(_configuration);

            services.ConfigureAuthroizationService(_configuration);

            if (_hostEnvironment.IsDevelopment())
            {
                services.ConfigureSwaggerService(_configuration);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.ConfigureCors(env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.ConfigureAuthentication(env);

            app.ConfigureAuthorization(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (_hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger(env);
            }
        }
    }
}
