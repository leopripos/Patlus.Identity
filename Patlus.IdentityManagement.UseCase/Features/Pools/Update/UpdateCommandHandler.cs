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
        private readonly ILogger<UpdateCommandHandler> _logger;
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public UpdateCommandHandler(ILogger<UpdateCommandHandler> logger, IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            this._logger = logger;
            this._dbService = dbService;
            this._timeService = timeService;
            this._mediator = mediator;

        }
        public async Task<Pool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));
            if (!request.HasName && !request.HasDescription) throw new ArgumentException($"One of {nameof(request.Name)} or {nameof(request.Description)} should be filled.");

            var currentTime = _timeService.Now;

            var entity = _dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), new { request.Id });
            }

            var valueChanges = new Dictionary<string, ValueChanged>();

            if (request.HasName)
            {
                valueChanges.Add(nameof(request.Name), new ValueChanged(entity.Name, request.Name));
                entity.Name = request.Name;
            }

            if (request.HasDescription)
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

            _dbService.Update(entity);

            await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                _logger.LogError(e, $"Error publish { nameof(UpdatedNotification) } when handle { nameof(UpdateCommand) } at { nameof(UpdateCommandHandler) }");
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return entity;
        }
    }
}
