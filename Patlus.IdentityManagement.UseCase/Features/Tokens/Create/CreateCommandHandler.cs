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

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Create
{
    public class CreateCommandHandler : ICommandFeatureHandler<CreateCommand, Token>
    {
        private readonly IMasterDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public CreateCommandHandler(IMasterDbContext dbContext, IPasswordService passwordService, ITokenService tokenService, ITimeService timeService, IMediator mediator)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _timeService = timeService;
            _mediator = mediator;
        }

        public async Task<Token> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            if (request.PoolId is null) throw new ArgumentNullException(nameof(request.PoolId));
            if (request.Name is null) throw new ArgumentNullException(nameof(request.Name));
            if (request.Password is null) throw new ArgumentNullException(nameof(request.Password));

            var query = _dbContext.Identities
                .Include(e => e.Pool)
                .Include(e => e.HostedAccount)
                .Where(
                    e => e.PoolId == request.PoolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false && e.HostedAccount.Name == request.Name
                );

            var entity = query.FirstOrDefault();

            if (entity is null || entity.HostedAccount is null || !_passwordService.ValidatePasswordHash(entity.HostedAccount.Password, request.Password))
            {
                throw new NotFoundException(nameof(Identity), new { request.PoolId, request.Name, Password = "****" });
            }

            var claims = new List<Claim>()
            {
                new Claim(SecurityClaimTypes.Subject, entity.Id.ToString()),
                new Claim(SecurityClaimTypes.Pool, entity.PoolId.ToString())
            };

            var token = _tokenService.Create(entity.AuthKey, claims);

            var notification = new CreatedNotification(entity.Id, token, request.RequestorId, _timeService.Now);

            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return token;
        }
    }
}
