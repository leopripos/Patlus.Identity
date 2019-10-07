using MediatR;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using Patlus.Identity.UseCase.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Pools.Commands.Create
{
    public class CreateCommandHandler : ICommandFeatureHandler<CreateCommand, Pool>
    {
        private readonly ILogger<CreateCommand> logger;
        private readonly IMetadataDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;
        private readonly IDbActivatorService dbActivatorService;

        public CreateCommandHandler(ILogger<CreateCommand> logger, IMetadataDbContext dbService, ITimeService timeService, IMediator mediator, IDbActivatorService dbActivatorService)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
            this.dbActivatorService = dbActivatorService;
        }

        public async Task<Pool> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var dbinfo = await dbActivatorService.Create(request.Id);

            var currentTime = timeService.Now;

            var entity = new Pool()
            {
                Id = request.Id,
                Active = request.Active.Value,
                CreatorId = request.RequestorId.Value,
                CreatedTime = currentTime,
                LastModifiedTime = currentTime,
            };

            entity.Database = new PoolDatabase()
            {
                Id = request.Id,
                ConnectionString = dbinfo.ConnectionString,
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