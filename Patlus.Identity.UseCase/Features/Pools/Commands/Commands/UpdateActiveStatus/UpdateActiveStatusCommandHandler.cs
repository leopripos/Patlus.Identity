using MediatR;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Common.UseCase.Services;
using Patlus.Identity.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Pools.Commands.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandHandler : ICommandFeatureHandler<UpdateActiveStatusCommand, Pool>
    {
        private readonly ILogger<UpdateActiveStatusCommand> logger;
        private readonly IMetadataDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;

        public UpdateActiveStatusCommandHandler(ILogger<UpdateActiveStatusCommand> logger, IMetadataDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
        }

        public async Task<Pool> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            var currentTime = timeService.Now;

            var entity = dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), request.Id);
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