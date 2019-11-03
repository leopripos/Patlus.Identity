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
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneById;
using Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    [ApiController]
    [Route("pools/{" + PoolResolver.POOL_ID_KEY + "}/tokens")]
    [Produces(MediaTypeNames.Application.Json)]
    public class TokensController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly ITimeService timeService;
        private readonly ITokenService tokenService;
        private readonly ITokenCacheService tokenCacheService;
        private readonly IPoolResolver poolResolver;

        public TokensController(IConfiguration configuration, IMediator mediator, ITimeService timeService, ITokenService tokenService, ITokenCacheService tokenCacheService, IPoolResolver poolResolver)
        {
            this.configuration = configuration;
            this.mediator = mediator;
            this.timeService = timeService;
            this.tokenService = tokenService;
            this.tokenCacheService = tokenCacheService;
            this.poolResolver = poolResolver;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateForm form)
        {
            var command = new GetOneBySecretQuery()
            {
                PoolId = poolResolver.Current.Id,
                Name = form.Name,
                Password = form.Password
            };

            try
            {
                var identity = await mediator.Send(command);

                if (identity.Active && identity.Pool.Active)
                {
                    var token = CreateToken(identity);
                    return Ok(token.Dto);
                }
                else {
                    return BadRequest(new ValidationErrorResultContent()
                    {
                        Message = "Identity inactive."
                    });
                }
            }
            catch (NotFoundException)
            {
                return BadRequest(new ValidationErrorResultContent() { 
                    Message = "Invalid name or password."
                });
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshForm form)
        {
            if (tokenService.ValidateRefreshToken(form.RefreshToken, out ClaimsPrincipal principal))
            {
                var tokenIdClaim = principal.FindFirst(TokenClaimType.TokenId);
                var subjectClaim = principal.FindFirst(TokenClaimType.Subject);
                var poolClaim = principal.FindFirst(TokenClaimType.Pool);


                var currentTokenId = Guid.Parse(tokenIdClaim.Value);
                var identityId = Guid.Parse(subjectClaim.Value);
                var poolId = Guid.Parse(poolClaim.Value);

                var command = new GetOneByIdQuery
                {
                    PoolId = poolId,
                    Id = identityId
                };

                try
                {
                    var identity = await mediator.Send(command);

                    if (tokenCacheService.HasToken(identity.Id, currentTokenId, identity.AuthKey.ToString()))
                    { 
                        ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token");
                    }
                    else if (identity.Active && identity.Pool.Active)
                    {
                        var newToken = CreateToken(identity);

                        RefreshToken(identity, newToken, currentTokenId);

                        return Ok(newToken.Dto);
                    }
                    else
                    {
                        return BadRequest(new ValidationErrorResultContent()
                        {
                            Message = "Identity inactive."
                        });
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

        private TokenGenerated CreateToken(Identity account)
        {
            var tokenId = Guid.NewGuid();

            var accessClaims = new List<Claim>
            {
                new Claim(TokenClaimType.TokenId, tokenId.ToString()),
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var refreshClaims = new List<Claim>
            {
                new Claim(TokenClaimType.TokenId, tokenId.ToString()),
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var accessTokenExpiredTime = timeService.Now.AddMinutes(configuration.GetValue<int>("Authentication:Token:AccessTokenTime"));
            var refreshTokenExpiredTime = timeService.Now.AddMinutes(configuration.GetValue<int>("Authentication:Token:RefreshTokenTime"));

            var tokenDto = new TokenDto()
            {
                AccessToken = tokenService.GenerateAccessToken(accessClaims, accessTokenExpiredTime, timeService.Now),
                RefreshToken = tokenService.GenerateRefreshToken(refreshClaims, refreshTokenExpiredTime, timeService.Now),
            };

            return new TokenGenerated()
            {
                TokenId = tokenId,
                RefreshExpiredTime = refreshTokenExpiredTime,
                Dto = tokenDto
            };
        }

        private void RefreshToken(Identity identity, TokenGenerated newToken, Guid currentTokenId)
        {
            tokenCacheService.Remove(identity.Id, currentTokenId);

            tokenCacheService.Set(identity.Id, newToken.TokenId, identity.AuthKey.ToString(), newToken.RefreshExpiredTime);
        }

        class TokenGenerated { 
            public Guid TokenId { get; set; }
            public DateTime RefreshExpiredTime { get; set; }
            public TokenDto Dto { get; set; }
        }
    }
}
