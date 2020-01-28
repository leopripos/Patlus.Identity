using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Notifications;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommandHandler : ICommandFeatureHandler<UpdateOwnPasswordCommand, Identity>
    {
        private readonly ILogger<UpdateOwnPasswordCommandHandler> _logger;
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;
        private readonly IPasswordService _passwordService;

        public UpdateOwnPasswordCommandHandler(ILogger<UpdateOwnPasswordCommandHandler> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator, IPasswordService passwordService)
        {
            _logger = logger;
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;
            _passwordService = passwordService;
        }

        public async Task<Identity> Handle(UpdateOwnPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.OldPassword)) throw new ArgumentException($"{nameof(request.OldPassword)} can not be empty, null or whitespace", nameof(request.OldPassword));
            if (string.IsNullOrWhiteSpace(request.NewPassword)) throw new ArgumentException($"{nameof(request.NewPassword)} can not be empty, null or whitespace", nameof(request.NewPassword));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = _timeService.Now;

            var query = _dbService.Identities.Include(e => e.HostedAccount).Where(e => e.Id == request.RequestorId);

            var entity = query.SingleOrDefault();

            if (entity == null || !_passwordService.ValidatePasswordHash(entity.HostedAccount!.Password, request.OldPassword))
            {
                throw new NotFoundException(nameof(HostedAccount), new { request.RequestorId, request.OldPassword });
            }

            var notification = new OwnPasswordUdpatedNotification(entity, new Dictionary<string, ValueChanged>(), request.RequestorId.Value, currentTime);

            entity.AuthKey = Guid.NewGuid();
            entity.LastModifiedTime = currentTime;

            entity.HostedAccount.Password = _passwordService.GeneratePasswordHash(request.NewPassword);
            entity.HostedAccount.LastModifiedTime = currentTime;

            _dbService.Update(entity);

            await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types, Justification: Error publishing notification unknown, but it should not interup action
            catch (Exception e)
            {
                _logger.LogError(e, $"Error publish { nameof(OwnPasswordUdpatedNotification) } when handle { nameof(UpdateOwnPasswordCommand) } at { nameof(UpdateOwnPasswordCommandHandler) }");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return entity;
        }
    }
}
