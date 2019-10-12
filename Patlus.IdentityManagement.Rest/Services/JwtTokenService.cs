using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Patlus.IdentityManagement.Rest.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Generate(IEnumerable<Claim> claims, DateTime expired, DateTime notBefore)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Authentication:Jwt:Issuer"],
                configuration["Authentication:Jwt:Audience"],
                claims,
                expires: expired,
                notBefore: notBefore,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
