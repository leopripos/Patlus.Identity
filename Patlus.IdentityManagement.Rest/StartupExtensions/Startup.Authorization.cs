using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.UseCase.Security;
using Patlus.IdentityManagement.Presentation.Auhtorization.Policies;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupAuthorization
    {
        public static void ConfigureAuthroizationService(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPoolPolicies();
                opt.AddIdentityPolicies();
                opt.AddTokenPolicies();
                opt.AddMePolicies();
            });
        }

        public static void ConfigureAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthorization();
        }

        private static void AddPoolPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(PoolPolicy.Read, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(PoolPolicy.Create, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(PoolPolicy.Update, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(PoolPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });
        }

        private static void AddIdentityPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(IdentityPolicy.Read, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(IdentityPolicy.Create, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(IdentityPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });
        }

        private static void AddTokenPolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });
        }

        private static void AddMePolicies(this AuthorizationOptions opt)
        {
            opt.AddPolicy(MePolicy.GetProfie, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });

            opt.AddPolicy(MePolicy.UpdatePassword, policy =>
            {
                policy.RequireClaim(SecurityClaimTypes.Subject);
                policy.RequireClaim(SecurityClaimTypes.Pool);
            });
        }
    }
}
