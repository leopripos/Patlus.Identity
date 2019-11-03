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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommandHandler : ICommandFeatureHandler<UpdateOwnPasswordCommand, HostedAccount>
    {
        private readonly ILogger<UpdateOwnPasswordCommand> logger;
        private readonly IMasterDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;
        private readonly IPasswordService passwordService;

        public UpdateOwnPasswordCommandHandler(ILogger<UpdateOwnPasswordCommand> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator, IPasswordService passwordService)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
            this.passwordService = passwordService;
        }

        public async Task<HostedAccount> Handle(UpdateOwnPasswordCommand request, CancellationToken cancellationToken)
        {
            var currentTime = timeService.Now;

            var query = dbService.HostedAccounts.Include(e => e.Identity).Where(e => e.Id == request.RequestorId);

            var entity = query.SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(HostedAccount), request.RequestorId);
            }

            var notification = new OwnPasswordUdpatedNotification
            {
                Entity = entity,
                Values = new Dictionary<string, ValueChanged>(),
                By = request.RequestorId.Value,
                Time = currentTime
            };

            entity.Password = passwordService.GeneratePasswordHash(request.NewPassword);
            entity.Identity.AuthKey = Guid.NewGuid();

            entity.LastModifiedTime = currentTime;

            dbService.Update(entity);

            await dbService.SaveChangesAsync(cancellationToken);

            try
            {
                await mediator.Publish(notification, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error publish { nameof(OwnPasswordUdpatedNotification) } when handle { nameof(UpdateOwnPasswordCommand) } at { nameof(UpdateOwnPasswordCommandHandler) }");
            }

            return entity;
        }
    }
}
