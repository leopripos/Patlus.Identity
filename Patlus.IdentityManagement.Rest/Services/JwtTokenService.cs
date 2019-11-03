using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration tokenConfiguration;
        public JwtTokenService(IConfigurationSection tokenConfiguration)
        {
            this.tokenConfiguration = tokenConfiguration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore)
        {
            return Generate(claims, expired, notBefore, tokenConfiguration.GetValue<string>("Key:Access"));
        }

        public string GenerateRefreshToken(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore)
        {
            return Generate(claims, expired, notBefore, tokenConfiguration.GetValue<string>("Key:Refresh"));
        }

        private string Generate(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore, string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                tokenConfiguration.GetValue<string>("Issuer"),
                tokenConfiguration.GetValue<string>("Audience"),
                claims,
                expires: expired,
                notBefore: notBefore,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateRefreshToken(string refreshToken, out ClaimsPrincipal principal)
        {
            var validatioParameter = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.GetValue<string>("Key:Refresh"))),
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = tokenConfiguration.GetValue<string>("Issuer"),
                ValidAudience = tokenConfiguration.GetValue<string>("Audience"),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validatioParameter, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception) { }

            principal = null;
            return false;
        }
    }
}
