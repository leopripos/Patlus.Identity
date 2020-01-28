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
    public static class StartupAuthentication
    {
        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfiguration = configuration.GetSection("Authentication:Token");

            services.AddScoped<IUserResolver, UserResolver>();
            services.AddTransient(typeof(JwtBearerAuthenticationEvent));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.GetValue<string>("Key:Access"))),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = tokenConfiguration.GetValue<string>("Issuer"),
                        ValidAudience = tokenConfiguration.GetValue<string>("Audience"),
                        ClockSkew = TimeSpan.Zero,
                        LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters parameter) =>
                        {
                            if (!(notBefore is null) && notBefore.Value > DateTime.UtcNow)
                                return false;

                            if (!(expires is null) && expires.Value < DateTime.UtcNow)
                                return false;

                            return true;
                        }
                    };

                    opt.EventsType = typeof(JwtBearerAuthenticationEvent);
                });
        }

        public static void ConfigureAuthentication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
        }
    }
}
