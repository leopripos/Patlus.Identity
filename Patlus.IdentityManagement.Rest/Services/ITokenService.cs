using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Patlus.IdentityManagement.Rest.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore);
        string GenerateRefreshToken(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore);
        bool ValidateRefreshToken(string refreshToken, out ClaimsPrincipal principal);
    }
}
