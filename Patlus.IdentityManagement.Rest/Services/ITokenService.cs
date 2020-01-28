using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Patlus.IdentityManagement.Rest.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, DateTimeOffset expired, DateTimeOffset notBefore);
        string GenerateRefreshToken(IEnumerable<Claim> claims, DateTimeOffset expired, DateTimeOffset notBefore);
        bool ValidateRefreshToken(string refreshToken, out ClaimsPrincipal principal);
    }
}
