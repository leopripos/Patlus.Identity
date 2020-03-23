using MediatR;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreateHostedCommandHandler : ICommandFeatureHandler<CreateHostedCommand, Identity>
    {
        private readonly ILogger<CreateHostedCommandHandler> _logger;
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;
        private readonly IPasswordService _passwordService;

        public CreateHostedCommandHandler(ILogger<CreateHostedCommandHandler> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator, IPasswordService passwordService)
        {
            _logger = logger;
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;
            _passwordService = passwordService;
        }

        public async Task<Identity> Handle(CreateHostedCommand request, CancellationToken cancellationToken)
        {
            if (request.PoolId is null) throw new ArgumentNullException(nameof(request.PoolId));
            if (request.Name is null) throw new ArgumentNullException(nameof(request.Name));
            if (request.AccountName is null) throw new ArgumentNullException(nameof(request.AccountName));
            if (request.AccountPassword is null) throw new ArgumentNullException(nameof(request.AccountPassword));
            if (request.Active is null) throw new ArgumentNullException(nameof(request.Active));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var pool = _dbService.Pools.Where(e => e.Id == request.PoolId && e.Archived == false).FirstOrDefault();

            if (pool is null) throw new NotFoundException(nameof(Pool), new { request.PoolId });

            if (_dbService.HostedAccounts.Where(e => e.Name == request.AccountName).Count() > 0)
            {
                throw new DuplicateEntryException(nameof(HostedAccount), new { request.AccountName });
            }

            var currentTime = _timeService.Now;

            var entity = new Identity()
            {
                Id = Guid.NewGuid(),
                AuthKey = Guid.NewGuid(),
                PoolId = pool.Id,
                Active = request.Active.Value,
                Name = request.Name,
                CreatorId = request.RequestorId.Value,
                CreatedTime = currentTime,
                LastModifiedTime = currentTime,
            };

            entity.HostedAccount = new HostedAccount()
            {
                Id = Guid.NewGuid(),
                Name = request.AccountName,
                Password = _passwordService.GeneratePasswordHash(request.AccountPassword),
                CreatorId = entity.CreatorId,
                CreatedTime = entity.CreatedTime,
                LastModifiedTime = currentTime,
            };

            _dbService.Add(entity);

            await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var notification = new HostedCreatedNotification(entity, request.RequestorId.Value, currentTime);

            try
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types, Justification: Error publishing notification unknown, but it should not interup action
            catch (Exception e)
            {
                _logger.LogError(e, $"Error publish {nameof(HostedCreatedNotification)} when handle { nameof(CreateHostedCommand) } at { nameof(CreateHostedCommandHandler) }");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return entity;
        }
    }
}
