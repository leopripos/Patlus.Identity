using MediatR;
using Microsoft.Extensions.Logging;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreateHostedCommandHandler : ICommandFeatureHandler<CreateHostedCommand, Identity>
    {
        private readonly ILogger<CreateHostedCommand> logger;
        private readonly IMasterDbContext dbService;
        private readonly ITimeService timeService;
        private readonly IMediator mediator;
        private readonly IPasswordService passwordService;

        public CreateHostedCommandHandler(ILogger<CreateHostedCommand> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator, IPasswordService passwordService)
        {
            this.logger = logger;
            this.dbService = dbService;
            this.timeService = timeService;
            this.mediator = mediator;
            this.passwordService = passwordService;
        }

        public async Task<Identity> Handle(CreateHostedCommand request, CancellationToken cancellationToken)
        {
            var currentTime = timeService.Now;

            var entity = new Identity()
            {
                Id = Guid.NewGuid(),
                AuthKey = Guid.NewGuid(),
                PoolId = request.PoolId.Value,
                Active = request.Active.Value,
                Name = request.Name,
                CreatorId = request.RequestorId.Value,
                CreatedTime = currentTime,
                LastModifiedTime = currentTime,
            };

            entity.HostedAccount = new HostedAccount()
            {
                Name = request.Name,
                Password = passwordService.GeneratePasswordHash(request.Password),
                CreatorId = entity.CreatorId,
                CreatedTime = entity.CreatedTime,
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
                logger.LogError(e, $"Error publish {nameof(CreatedNotification)} when handle { nameof(CreateHostedCommand) } at { nameof(CreateHostedCommandHandler) }");
            }

            return entity;
        }
    }
}