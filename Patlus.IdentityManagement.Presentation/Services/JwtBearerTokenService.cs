using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Patlus.Common.UseCase.Security;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Cache.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using Patlus.IdentityManagement.Presentation.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Patlus.IdentityManagement.Presentation.Services
{
    public class JwtBearerTokenService : ITokenService
    {
        private readonly IOptions<AuthenticationOptions> _authOptions;
        private readonly ITimeService _timeService;
        private readonly ITokenCacheService _tokenCacheService;

        public JwtBearerTokenService(IOptions<AuthenticationOptions> authOptions, ITimeService timeService, ITokenCacheService tokenCacheService)
        {
            _authOptions = authOptions;
            _timeService = timeService;
            _tokenCacheService = tokenCacheService;
        }

        public Token Create(Guid authKey, IList<Claim> claims)
        {
            var id = Guid.NewGuid();

            var token = new Token()
            {
                Id = id,
                Scheme = SecurityTokenTypes.Bearer,
                Access = CreateAccessToken(id, claims),
                Refresh = CreateRefreshToken(id, authKey, claims),
                CreatedTime = _timeService.Now
            };

            return token;
        }

        public bool TryParseRefreshToken(string refreshToken, out ClaimsPrincipal principal)
        {
            var refreshTokenOptions = _authOptions.Value.RefreshToken;

            var validatioParameter = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenOptions.Key)),
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = refreshTokenOptions.Issuer,
                ValidAudience = refreshTokenOptions.Audience,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validatioParameter, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception e) when (
                e is ArgumentException
                || e is SecurityTokenDecryptionFailedException
                || e is SecurityTokenEncryptionKeyNotFoundException
                || e is SecurityTokenException
                || e is SecurityTokenExpiredException
                || e is SecurityTokenInvalidAudienceException
                || e is SecurityTokenInvalidLifetimeException
                || e is SecurityTokenInvalidSignatureException
                || e is SecurityTokenNoExpirationException
                || e is SecurityTokenNotYetValidException
                || e is SecurityTokenReplayAddFailedException
                || e is SecurityTokenReplayDetectedException
               )
            { }

            principal = null!;
            return false;
        }

        public bool ValidateRefreshToken(Guid authKey, ClaimsPrincipal principal)
        {
            var tokenIdClaim = principal.FindFirst(SecurityClaimTypes.TokenId);

            if (tokenIdClaim is null)
            {
                return false;
            }
            else
            {
                var currentTokenId = Guid.Parse(tokenIdClaim.Value);

                return _tokenCacheService.HasToken(currentTokenId, authKey);
            }
        }

        private string CreateAccessToken(Guid id, IList<Claim> claims)
        {
            var tokenOptions = _authOptions.Value.AccessToken;

            var finalClaims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.TokenId, id.ToString()),
                new Claim(SecurityClaimTypes.Version, tokenOptions.Version),
            };

            finalClaims.AddRange(claims);

            var expiredTime = _timeService.Now.AddMinutes(tokenOptions.Duration);

            return Generate(finalClaims, expiredTime, _timeService.Now, tokenOptions.Key, tokenOptions.Issuer, tokenOptions.Audience);
        }

        private string CreateRefreshToken(Guid id, Guid authKey, IList<Claim> claims)
        {
            var tokenOptions = _authOptions.Value.RefreshToken;

            var finalClaims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.TokenId, id.ToString()),
                new Claim(SecurityClaimTypes.Version, tokenOptions.Version),
            };

            finalClaims.AddRange(claims);

            var expiredTime = _timeService.Now.AddMinutes(tokenOptions.Duration);

            var jwtToken = Generate(finalClaims, expiredTime, _timeService.Now, tokenOptions.Key, tokenOptions.Issuer, tokenOptions.Audience);

            _tokenCacheService.Set(id, authKey, expiredTime);

            return jwtToken;
        }

        private string Generate(IEnumerable<Claim> claims, DateTimeOffset expired, DateTimeOffset notBefore, string key, string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expired.UtcDateTime,
                notBefore: notBefore.UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
