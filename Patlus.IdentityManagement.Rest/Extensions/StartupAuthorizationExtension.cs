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
                opt.AddTokenPolicies();
                opt.AddMePolicies();
            });
        }

        public static void UseAuthorizationFeature(this IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseAuthorization();
        }

        private static void AddPoolPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(PoolPolicy.Read, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });

            opt.AddPolicy(PoolPolicy.Create, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });

            opt.AddPolicy(PoolPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });
        }

        private static void AddIdentityPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(IdentityPolicy.Read, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });

            opt.AddPolicy(IdentityPolicy.Create, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });

            opt.AddPolicy(IdentityPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });

            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });
        }

        private static void AddTokenPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });
        }

        private static void AddMePolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(MePolicy.UpdatePassword, policy =>
            {
                policy.RequireClaim(TokenClaimType.Subject);
                policy.RequireClaim(TokenClaimType.Pool);
            });
        }
    }
}
