using MediatR;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Create
{
    public class CreateCommandHandler : ICommandFeatureHandler<CreateCommand, Pool>
    {
        private readonly ILogger<CreateCommand> logger;
        private readonly IMasterDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;

        public CreateCommandHandler(ILogger<CreateCommand> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
        }

        public async Task<Pool> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var currentTime = timeService.Now;

            var entity = new Pool()
            {
                Id = Guid.NewGuid(),
                Active = request.Active.Value,
                Name = request.Name,
                Description = request.Description,
                CreatorId = request.RequestorId.Value,
                CreatedTime = currentTime,
                LastModifiedTime = currentTime,
            };

            dbService.Add(entity);

            await dbService.SaveChangesAsync(cancellationToken);

            var notification = new CreatedNotification
            {
                Entity = entity,
                By = request.RequestorId.Value,
                Time = currentTime
            };

            try
            {
                await mediator.Publish(notification, cancellationToken);
            }
            catch(Exception e) {
                logger.LogError(e, $"Error publish {nameof(CreatedNotification)} when handle { nameof(CreateCommand) } at { nameof(CreateCommandHandler) }");
            }

            return entity;
        }
    }
}