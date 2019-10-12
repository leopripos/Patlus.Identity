using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.Rest.Policies;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetOneBySecret;
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

        [HttpPut]
        [Authorize(Policy = TokenPolicy.Refresh)]
        public async Task<TokenDto> Refresh()
        {
            var subject = User.FindFirst(ClaimType.Subject);
            var accountId = Guid.Parse(subject.Value);

            var command = new UseCase.Features.Accounts.Queries.GetOneById.GetOneByIdQuery
            {
                Id = accountId
            };

            var account = await mediator.Send(command);

            return CreateToken(account);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<TokenDto> Create([FromBody] CreateForm form)
        {
            var command = new GetOneBySecretQuery
            {
                Name = form.Name,
                Password = form.Password
            };

            var account = await mediator.Send(command);

            return CreateToken(account);
        }

        private TokenDto CreateToken(Account account)
        {
            var accessClaims = new List<Claim>
            {
                new Claim(ClaimType.AccessType, TokenType.AccessToken),
                new Claim(ClaimType.Version, "1.0"),
                new Claim(ClaimType.Subject, account.Id.ToString()),
                new Claim(ClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var refreshClaims = new List<Claim>
            {
                new Claim(ClaimType.AccessType, TokenType.RefreshToken),
                new Claim(ClaimType.Version, "1.0"),
                new Claim(ClaimType.Subject, account.Id.ToString()),
                new Claim(ClaimType.Pool, poolResolver.Current.Id.ToString())
            };

            var accessTokenTime = int.Parse(configuration["Authentication:Jwt:AccessTokenTime"]);
            var refreshTokenTime = int.Parse(configuration["Authentication:Jwt:RefreshTokenTime"]);

            return new TokenDto()
            {
                AccessToken = tokenGenerator.Generate(accessClaims, timeService.Now.AddMinutes(accessTokenTime), timeService.Now),
                RefreshToken = tokenGenerator.Generate(refreshClaims, timeService.Now.AddMinutes(refreshTokenTime), timeService.Now),
            };
        }
    }
}
