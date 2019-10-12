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

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandHandler : ICommandFeatureHandler<UpdateActiveStatusCommand, Identity>
    {
        private readonly ILogger<UpdateActiveStatusCommand> logger;
        private readonly IMasterDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;

        public UpdateActiveStatusCommandHandler(ILogger<UpdateActiveStatusCommand> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
        }

        public async Task<Identity> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            var currentTime = timeService.Now;

            var query = dbService.Identities.Where(e => e.Id == request.Id);

            if (request.PoolId.HasValue)
            {
                query = query.Where(e => e.PoolId == request.PoolId);
            }

            var entity = query.SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Identity), request.Id);
            }

            if (entity.Active != request.Active.Value)
            {
                var notification = new ActiveStatusUpdatedNotification
                {
                    Entity = entity,
                    Old = entity.Active,
                    New = request.Active.Value,
                    By = request.RequestorId.Value,
                    Time = currentTime
                };

                entity.Active = request.Active.Value;
                entity.LastModifiedTime = currentTime;

                dbService.Update(entity);

                await dbService.SaveChangesAsync(cancellationToken);

                try
                {
                    await mediator.Publish(notification, cancellationToken);
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error publish { nameof(ActiveStatusUpdatedNotification) } when handle { nameof(UpdateActiveStatusCommand) } at { nameof(UpdateActiveStatusCommandHandler) }");
                }
            }

            return entity;
        }
    }
}