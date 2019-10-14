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
using Patlus.IdentityManagement.Rest.Filters.Exceptions;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.Rest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => {
                    opt.Filters.Add(new NotFoundExceptionFilter());
                    opt.Filters.Add(new ValidationExceptionFilter());
                });

            services.AddMediatR(ModuleProfile.GetBundles());
            services.AddValidatorsFromAssemblies(ModuleProfile.GetBundles());
            services.AddAutoMapper(GetType().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<ITimeService>(new TimeService());
            services.AddSingleton<ITokenService>(new JwtTokenService(Configuration));
            services.AddSingleton<IPasswordService>(new HMACSHA1PasswordService(Configuration));

            services.AddHttpContextAccessor();
            services.AddScoped<IPoolResolver, PoolResolver>();

            services.AddDbContext<IMasterDbContext, MasterDbContext>(opt => {
                opt.UseSqlServer(Configuration["Database:Connection"], x => x.MigrationsAssembly("Patlus.IdentityManagement.Persistence"));
            });

            services.ConfigureAuthenticationService(this.Configuration);

            services.ConfigureAuthroizationService(this.Configuration);

            services.ConfigureSwaggerService(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthenticationFeature(env);

            app.UseAuthorizationFeature(env);

            app.UseSwaggerFeature(env);
        }
    }
}
