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

namespace Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandHandler : ICommandFeatureHandler<UpdateActiveStatusCommand, Pool>
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

        public async Task<Pool> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.Active is null) throw new ArgumentNullException(nameof(request.Active));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = timeService.Now;

            var entity = dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), request.Id);
            }

            if (entity.Active != request.Active.Value)
            {
                var notification = new ActiveStatusUpdatedNotification(entity, entity.Active, request.Active.Value, request.RequestorId.Value, currentTime);

                entity.Active = request.Active.Value;
                entity.LastModifiedTime = currentTime;

                dbService.Update(entity);

                await dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                try
                {
                    await mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
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
