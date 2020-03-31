using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Patlus.Common.Presentation.Security;
using Patlus.Common.UseCase.Security;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Patlus.IdentityManagement.Presentation.Services
{
    public class JwtBearerTokenService : ITokenService
    {
        private readonly IOptions<AccessTokenOptions> _accessTokenOptions;
        private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions;
        private readonly IIdentifierService _identifierService;
        private readonly ITimeService _timeService;
        private readonly ITokenStorageService _tokenCacheService;

        public JwtBearerTokenService(
            IOptions<AccessTokenOptions> accessTokenOptions, 
            IOptions<RefreshTokenOptions> refreshTokenOptions, 
            IIdentifierService identifierService,
            ITimeService timeService, 
            ITokenStorageService tokenStorageService
        ) {
            _accessTokenOptions = accessTokenOptions;
            _refreshTokenOptions = refreshTokenOptions;
            _identifierService = identifierService;
            _timeService = timeService;
            _tokenCacheService = tokenStorageService;
        }

        public Token Create(Guid authKey, IList<Claim> claims)
        {
            var id = _identifierService.NewGuid();

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
            var options = _refreshTokenOptions.Value;

            var validatioParameter = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)),
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ClockSkew = TimeSpan.Zero,
                LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters parameter) =>
                {
                    if (!(notBefore is null) && notBefore.Value > _timeService.NowDateTime.ToUniversalTime())
                        return false;

                    if (!(expires is null) && expires.Value <= _timeService.NowDateTime.ToUniversalTime())
                        return false;

                    return true;
                }
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
            var options = _accessTokenOptions.Value;

            var finalClaims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.TokenId, id.ToString()),
                new Claim(SecurityClaimTypes.Version, options.Version),
            };

            finalClaims.AddRange(claims);

            var expiredTime = _timeService.Now.AddMinutes(options.Duration);

            return Generate(finalClaims, expiredTime, _timeService.Now, options.Key, options.Issuer, options.Audience);
        }

        private string CreateRefreshToken(Guid id, Guid authKey, IList<Claim> claims)
        {
            var options = _refreshTokenOptions.Value;

            var finalClaims = new List<Claim>
            {
                new Claim(SecurityClaimTypes.TokenId, id.ToString()),
                new Claim(SecurityClaimTypes.Version, options.Version),
            };

            finalClaims.AddRange(claims);

            var expiredTime = _timeService.Now.AddMinutes(options.Duration);

            var jwtToken = Generate(finalClaims, expiredTime, _timeService.Now, options.Key, options.Issuer, options.Audience);

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
