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
        private readonly ITokenService tokenGenerator;
        private readonly IPoolResolver poolResolver;

        public TokensController(IConfiguration configuration, IMediator mediator, ITimeService timeService, ITokenService tokenGenerator, IPoolResolver poolResolver)
        {
            this.configuration = configuration;
            this.mediator = mediator;
            this.timeService = timeService;
            this.tokenGenerator = tokenGenerator;
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
                    return Ok(CreateToken(identity));
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
            if (tokenGenerator.ValidateRefreshToken(form.RefreshToken, out ClaimsPrincipal principal))
            {
                var subjectClaim = principal.FindFirst(TokenClaimType.Subject);
                var poolClaim = principal.FindFirst(TokenClaimType.Pool);

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

                    if (identity.Active && identity.Pool.Active)
                    {
                        return Ok(CreateToken(identity));
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

                    return BadRequest(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError(nameof(form.RefreshToken), "Invalid refresh token");

                return BadRequest(ModelState);
            }
        }

        private TokenDto CreateToken(Identity account)
        {
            var accessClaims = new List<Claim>
            {
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var refreshClaims = new List<Claim>
            {
                new Claim(TokenClaimType.Version, "1.0"),
                new Claim(TokenClaimType.Subject, account.Id.ToString()),
                new Claim(TokenClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var accessTokenTime = int.Parse(configuration["Authentication:Jwt:AccessTokenTime"]);
            var refreshTokenTime = int.Parse(configuration["Authentication:Jwt:RefreshTokenTime"]);

            return new TokenDto()
            {
                AccessToken = tokenGenerator.GenerateAccessToken(accessClaims, timeService.Now.AddMinutes(accessTokenTime), timeService.Now),
                RefreshToken = tokenGenerator.GenerateRefreshToken(refreshClaims, timeService.Now.AddMinutes(refreshTokenTime), timeService.Now),
            };
        }
    }
}
