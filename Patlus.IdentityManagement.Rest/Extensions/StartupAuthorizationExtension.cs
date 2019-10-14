using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.IdentityManagement.Rest.Auhtorization.Policies;
using Patlus.IdentityManagement.Rest.Authentication.Token;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupAuthorizationExtension
    {
        public static void ConfigureAuthroizationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPoolPolicies();
                opt.AddIdentityPolicies();
            });
        }

        public static void UseAuthorizationFeature(this IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseAuthorization();
        }

        private static void AddPoolPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(PoolPolicy.Read, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });

            opt.AddPolicy(PoolPolicy.Create, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });

            opt.AddPolicy(PoolPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });
        }

        private static void AddIdentityPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(IdentityPolicy.Read, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });

            opt.AddPolicy(IdentityPolicy.Create, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });

            opt.AddPolicy(IdentityPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.AccessToken);
            });

            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(TokenClaimType.AccessType, TokenAccessType.RefreshToken);
            });
        }
    }
}
