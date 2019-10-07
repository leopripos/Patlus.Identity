using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Patlus.Identity.Rest.Services
{
    public interface ITokenService
    {
        string Generate(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore);
    }
}
