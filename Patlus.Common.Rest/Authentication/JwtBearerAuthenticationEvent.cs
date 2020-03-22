using Microsoft.AspNetCore.Authentication.JwtBearer;
using Patlus.Common.UseCase.Security;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Patlus.Common.Rest.Authentication
{
    public class JwtBearerAuthenticationEvent : JwtBearerEvents
    {
        private readonly IUserResolver _userResolver;

        public JwtBearerAuthenticationEvent(IUserResolver userResolver)
        {
            _userResolver = userResolver;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var accessToken = (context.SecurityToken as JwtSecurityToken)!;

            if (accessToken != null)
            {
                if (!(context.Principal.Identity is ClaimsIdentity identity))
                {
                    context.Fail("Identity not found");
                }
                else
                {
                    var subjectClaim = identity.FindFirst(SecurityClaimTypes.Subject);
                    var poolIdClaim = identity.FindFirst(SecurityClaimTypes.Pool);

                    if (
                        subjectClaim is null
                        || poolIdClaim is null
                        || !Guid.TryParse(subjectClaim.Value, out Guid userId)
                        || !Guid.TryParse(poolIdClaim.Value, out Guid poolId)
                    )
                    {
                        context.Fail("Invalid access token");
                    }
                    else
                    {
                        _userResolver.Initialize(new User(userId, poolId));
                    }
                }
            }

            return base.TokenValidated(context);
        }
    }
}
