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
        private readonly ILogger<CreateCommandHandler> _logger;
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public CreateCommandHandler(ILogger<CreateCommandHandler> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            _logger = logger;
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;
        }

        public async Task<Pool> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            if (request.Name is null) throw new ArgumentNullException(nameof(request.Name));
            if (request.Description is null) throw new ArgumentNullException(nameof(request.Description));
            if (request.Active is null) throw new ArgumentNullException(nameof(request.Active));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = _timeService.Now;

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

            _dbService.Add(entity);

            await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var notification = new CreatedNotification(entity, request.RequestorId.Value, currentTime);

            try
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types, Justification: Error when publishing notification cannot be predicted, but it should not interup action
            catch (Exception e)
            {
                _logger.LogError(e, $"Error publish {nameof(CreatedNotification)} when handle { nameof(CreateCommand) } at { nameof(CreateCommandHandler) }");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return entity;
        }
    }
}
