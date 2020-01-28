using MediatR;
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

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommandHandler : ICommandFeatureHandler<UpdateCommand, Pool>
    {
        private readonly ILogger<UpdateCommand> logger;
        private readonly IMasterDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;

        public UpdateCommandHandler(ILogger<UpdateCommand> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;

        }
        public async Task<Pool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = timeService.Now;

            var entity = dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), request.Id);
            }

            var valueChanges = new Dictionary<string, ValueChanged>();

            if (request.Name != null)
            {
                valueChanges.Add(nameof(request.Name), new ValueChanged(entity.Name, request.Name));
                entity.Name = request.Name;
            }

            if (request.Description != null)
            {
                valueChanges.Add(nameof(request.Description), new ValueChanged(entity.Description, request.Description));
                entity.Description = request.Description;
            }

            var notification = new UpdatedNotification(
                entity,
                valueChanges,
                request.RequestorId.Value,
                currentTime
            );

            entity.LastModifiedTime = currentTime;

            dbService.Update(entity);

            await dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error publish { nameof(UpdatedNotification) } when handle { nameof(UpdateCommand) } at { nameof(UpdateCommandHandler) }");
            }

            return entity;
        }
    }
}
