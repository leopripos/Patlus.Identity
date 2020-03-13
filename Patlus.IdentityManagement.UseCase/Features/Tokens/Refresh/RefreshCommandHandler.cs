using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Security;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh
{
    public class RefreshCommandHandler : ICommandFeatureHandler<RefreshCommand, Token>
    {
        private readonly ILogger<RefreshCommandHandler> _logger;
        private readonly IMasterDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public RefreshCommandHandler(ILogger<RefreshCommandHandler> logger, IMasterDbContext dbContext, ITokenService tokenService, ITimeService timeService, IMediator mediator)
        {
            _logger = logger;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _timeService = timeService;
            _mediator = mediator;
        }

        public async Task<Token> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            if (request.RefreshToken is null) throw new ArgumentNullException(nameof(request.RefreshToken));

            if (_tokenService.TryParseRefreshToken(request.RefreshToken!, out var principal))
            {
                var tokenIdentityId = Guid.Parse(principal.FindFirst(SecurityClaimTypes.Subject).Value);
                var tokenPoolId = Guid.Parse(principal.FindFirst(SecurityClaimTypes.Pool).Value);

                var query = _dbContext.Identities
                .Include(e => e.Pool)
                .Include(e => e.HostedAccount)
                .Where(
                    e => e.Id == tokenIdentityId && e.PoolId == tokenPoolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false
                );

                var entity = query.FirstOrDefault();

                if (entity is null || entity.HostedAccount is null || !_tokenService.ValidateRefreshToken(entity.AuthKey, principal))
                {
                    throw new NotFoundException(nameof(Identity), new { PoolId = tokenPoolId, Id = tokenIdentityId });
                }

                var claims = new List<Claim>()
                {
                    new Claim(SecurityClaimTypes.Subject, entity.Id.ToString()),
                    new Claim(SecurityClaimTypes.Pool, entity.PoolId.ToString())
                };

                var token = _tokenService.Create(entity.AuthKey, claims);

                try
                {
                    var notification = new RefreshedNotification(token, request.RequestorId, _timeService.Now);

                    await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
                }
#pragma warning disable CA1031 // Do not catch general exception types, Justification: Error when publishing notification cannot be predicted, but it should not interup action
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error publish {nameof(RefreshedNotification)} when handle { nameof(RefreshCommand) } at { nameof(RefreshCommandHandler) }");
                }
#pragma warning restore CA1031 // Do not catch general exception types

                return token;
            }

            throw new ArgumentException("Invalid refreh token.", nameof(request.RefreshToken));
        }
    }
}
