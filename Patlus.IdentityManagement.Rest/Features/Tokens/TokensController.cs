using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Rest.Authentication.Token;
using Patlus.IdentityManagement.Rest.Responses.Content;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOne;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    [ApiController]
    [Route("tokens")]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ITimeService _timeService;
        private readonly ITokenService _tokenService;
        private readonly ITokenCacheService _tokenCacheService;

        public TokensController(IConfiguration configuration, IMediator mediator, ITimeService timeService, ITokenService tokenService, ITokenCacheService tokenCacheService)
        {
            _configuration = configuration;
            _mediator = mediator;
            _timeService = timeService;
            _tokenService = tokenService;
            _tokenCacheService = tokenCacheService;
        }

        /// <summary>
        /// Create authentication token by passing user credential
        /// </summary>
        /// <param name="form">Create form</param>
        /// <returns>Authentication Token</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Create([FromBody] CreateForm form)
        {
            var command = new GetOneBySecretQuery()
            {
                PoolId = form.PoolId,
                Name = form.Name,
                Password = form.Password
            };

            try
            {
                var identity = await _mediator.Send(command).ConfigureAwait(false);

                if (identity.Pool is null) throw new Exception($"{nameof(identity.Pool)} is null");

                if (identity.Active && identity.Pool.Active)
                {
                    var token = CreateBearerToken(identity.Pool.Id, identity);

                    return Ok(token.TokenDto);
                }
                else
                {
                    return BadRequest(new ValidationErrorResultContent("Identity inactive."));
                }
            }
            catch (NotFoundException)
            {
                return BadRequest(new ValidationErrorResultContent("Invalid name or password."));
            }
        }

        /// <summary>
        /// Refresh the token by passing refresh token
        /// </summary>
        /// <param name="form">Refresh Form</param>
        /// <returns>Authentication token</returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDto>> Refresh([FromBody] RefreshForm form)
        {
            if (form.RefreshToken is null) throw new ArgumentNullException(nameof(form.RefreshToken));

            if (_tokenService.ValidateRefreshToken(form.RefreshToken, out ClaimsPrincipal? principal))
            {
                if (principal is null) throw new Exception($"{nameof(principal)} is null.");

                var tokenIdClaim = principal.FindFirst(TokenClaimType.TokenId);
                var subjectClaim = principal.FindFirst(TokenClaimType.Subject);
                var poolClaim = principal.FindFirst(TokenClaimType.Pool);

                var currentTokenId = Guid.Parse(tokenIdClaim.Value);
                var tokenIdentityId = Guid.Parse(subjectClaim.Value);
                var tokenPoolId = Guid.Parse(poolClaim.Value);

                var command = new GetOneQuery
                {
                    Condition = (e => e.PoolId == tokenPoolId && e.Id == tokenIdentityId)
                };

                try
                {
                    var identity = await _mediator.Send(command).ConfigureAwait(false);

                    if (identity.Pool is null) throw new Exception($"{nameof(identity.Pool)} is null");

                    if (_tokenCacheService.HasToken(identity.Id, currentTokenId, identity.AuthKey.ToString()))
                    {
                        ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token");
                    }
                    else if (identity.Active && identity.Pool.Active)
                    {
                        var newToken = CreateBearerToken(tokenPoolId, identity);

                        RefreshToken(identity, newToken, currentTokenId);

                        return Ok(newToken.TokenDto);
                    }
                    else
                    {
                        return BadRequest(new ValidationErrorResultContent("Identity inactive."));
                    }
                }
                catch (NotFoundException)
                {
                    ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token");
            }


            return BadRequest(ModelState);
        }

        private TokenGenerated CreateBearerToken(Guid poolId, Identity account)
        {
            var tokenId = Guid.NewGuid();

            var accessClaims = new List<Claim>
            {
                new Claim(TokenClaimType.TokenId, tokenId.ToString()),
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolId.ToString())
            };

            var refreshClaims = new List<Claim>
            {
                new Claim(TokenClaimType.TokenId, tokenId.ToString()),
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolId.ToString())
            };

            var accessTokenExpiredTime = _timeService.Now.AddMinutes(_configuration.GetValue<int>("Authentication:Token:AccessTokenTime"));
            var refreshTokenExpiredTime = _timeService.Now.AddMinutes(_configuration.GetValue<int>("Authentication:Token:RefreshTokenTime"));

            var tokenDto = new TokenDto()
            {
                Scheme = "Bearer",
                AccessToken = _tokenService.GenerateAccessToken(accessClaims, accessTokenExpiredTime, _timeService.Now),
                RefreshToken = _tokenService.GenerateRefreshToken(refreshClaims, refreshTokenExpiredTime, _timeService.Now),
            };

            return new TokenGenerated(tokenId, refreshTokenExpiredTime, tokenDto);
        }

        private void RefreshToken(Identity identity, TokenGenerated newToken, Guid currentTokenId)
        {
            _tokenCacheService.Remove(identity.Id, currentTokenId);

            _tokenCacheService.Set(identity.Id, newToken.TokenId, identity.AuthKey.ToString(), newToken.RefreshExpiredTime);
        }

        class TokenGenerated
        {
            public Guid TokenId { get; set; }
            public DateTimeOffset RefreshExpiredTime { get; set; }
            public TokenDto TokenDto { get; set; }

            public TokenGenerated(Guid tokenId, DateTimeOffset refreshExpiredTime, TokenDto dto)
            {
                this.TokenId = tokenId;
                this.RefreshExpiredTime = refreshExpiredTime;
                this.TokenDto = dto;
            }
        }
    }
}
