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
        private readonly ILogger<UpdateActiveStatusCommandHandler> _logger;
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public UpdateActiveStatusCommandHandler(ILogger<UpdateActiveStatusCommandHandler> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            _logger = logger;
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;
        }

        public async Task<Pool> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.Active is null) throw new ArgumentNullException(nameof(request.Active));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = _timeService.Now;

            var entity = _dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), new { request.Id });
            }

            var notification = new ActiveStatusUpdatedNotification(entity, entity.Active, request.Active.Value, request.RequestorId.Value, currentTime);

            entity.Active = request.Active.Value;
            entity.LastModifiedTime = currentTime;

            _dbService.Update(entity);

            await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                _logger.LogError(e, $"Error publish { nameof(ActiveStatusUpdatedNotification) } when handle { nameof(UpdateActiveStatusCommand) } at { nameof(UpdateActiveStatusCommandHandler) }");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return entity;
        }
    }
}
