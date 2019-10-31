using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Patlus.IdentityManagement.Rest.Authentication.Token;

namespace Patlus.IdentityManagement.Rest.Authentication
{
    public class JwtBearerAuthenticationEvent : JwtBearerEvents
    {
        private readonly IUserResolver userResolver;

        public JwtBearerAuthenticationEvent(IUserResolver userResolver)
        {
            this.userResolver = userResolver;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var accessToken = context.SecurityToken as JwtSecurityToken;

            if (accessToken != null)
            {
                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;

                var subjectClaim = identity.FindFirst(TokenClaimType.Subject);

                if (subjectClaim == null)
                {
                    context.Fail("Invalid access token");
                }
                else if (Guid.TryParse(subjectClaim.Value, out Guid userId))
                {
                    this.userResolver.Initialize(userId);
                }
                else
                {
                    context.Fail("Invalid access token");
                }
            }

            return base.TokenValidated(context);
        }
    }
}
