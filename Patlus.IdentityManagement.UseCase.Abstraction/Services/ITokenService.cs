using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Patlus.IdentityManagement.UseCase.Services
{
    public interface ITokenService
    {
        Token Create(Guid authKey, IList<Claim> claims);
        bool TryParseRefreshToken(string refreshToken, out ClaimsPrincipal principal);
        bool ValidateRefreshToken(Guid authKey, ClaimsPrincipal principal);
    }
}
