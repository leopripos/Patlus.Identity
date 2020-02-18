using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.Rest.Extensions;
using Patlus.IdentityManagement.Rest.Filters.Actions;
using Patlus.IdentityManagement.Rest.Filters.Exceptions;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.Rest.StartupExtensions;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;

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
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new NotFoundExceptionFilter());
                opt.Filters.Add(new ValidationExceptionFilter());
            });

            services.AddScoped<ValidPoolFilter>();

            services.AddMediatR(ModuleProfile.GetBundles());
            services.AddValidatorsFromAssemblies(ModuleProfile.GetBundles());
            services.AddAutoMapper(GetType().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<ITimeService, TimeService>();
            services.AddSingleton<ITokenCacheService, TokenCacheService>();
            services.AddSingleton<ITokenService>(new JwtTokenService(_configuration.GetSection("Authentication:Token")));
            services.AddSingleton<IPasswordService>(new HMACSHA1PasswordService(_configuration.GetSection("Authentication:Password")));

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

            services.ConfigureSwaggerService(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCors(env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.ConfigureAuthentication(env);

            app.ConfigureAuthorization(env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ConfigureSwagger(env);
        }
    }
}
