using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Patlus.IdentityManagement.Rest.Authentication;
using System;
using System.Text;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupAuthenticationExtension
    {
        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserResolver, UserResolver>();
            services.AddTransient(typeof(JwtBearerAuthenticationEvent));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"])),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = configuration["Authentication:Jwt:Issuer"],
                        ValidAudience = configuration["Authentication:Jwt:Audience"],
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    opt.EventsType = typeof(JwtBearerAuthenticationEvent);
                });
        }

        public static void UseAuthenticationFeature(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
        }
    }
}
