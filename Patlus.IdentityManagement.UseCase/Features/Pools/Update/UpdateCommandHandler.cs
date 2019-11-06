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
            var currentTime = timeService.Now;

            var entity = dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), request.Id);
            }

            var valueChanges = new Dictionary<string, ValueChanged>();

            if (request.HasName)
            {
                valueChanges.Add(nameof(request.Name), new ValueChanged()
                {
                    Old = entity.Name,
                    New = request.Name
                });

                entity.Name = request.Name;
            }

            if (request.HasDescription)
            {
                valueChanges.Add(nameof(request.Description), new ValueChanged()
                {
                    Old = entity.Description,
                    New = request.Description
                });

                entity.Name = request.Name;
            }

            var notification = new UpdatedNotification
            {
                Entity = entity,
                Values = valueChanges,
                By = request.RequestorId.Value,
                Time = currentTime
            };
            
            entity.LastModifiedTime = currentTime;

            dbService.Update(entity);

            await dbService.SaveChangesAsync(cancellationToken);

            try
            {
                await mediator.Publish(notification, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error publish { nameof(UpdatedNotification) } when handle { nameof(UpdateCommand) } at { nameof(UpdateCommandHandler) }");
            }

            return entity;
        }
    }
}
