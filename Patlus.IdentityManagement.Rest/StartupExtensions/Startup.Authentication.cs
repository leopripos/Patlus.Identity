using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Patlus.Common.Presentation;
using Patlus.Common.Rest.Authentication;
using Patlus.IdentityManagement.Presentation.Services;
using System;
using System.Text;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupAuthentication
    {
        public static void ConfigureAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordOptions>(options =>
            {
                configuration.GetSection("Authentication:Password").Bind(options);
            });

            services.Configure<AccessTokenOptions>(options =>
            {
                configuration.GetSection("Authentication:AccessToken").Bind(options);
            });
            
            services.Configure<RefreshTokenOptions>(options =>
            {
                configuration.GetSection("Authentication:RefreshToken").Bind(options);
            });

            services.AddScoped<IUserResolver, UserResolver>();
            services.AddTransient(typeof(JwtBearerAuthenticationEvent));

            var accessTokenOptions = new AccessTokenOptions();
            configuration.GetSection("Authentication:AccessToken").Bind(accessTokenOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessTokenOptions.Key)),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = accessTokenOptions.Issuer,
                        ValidAudience = accessTokenOptions.Audience,
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

        public static void ConfigureAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}
