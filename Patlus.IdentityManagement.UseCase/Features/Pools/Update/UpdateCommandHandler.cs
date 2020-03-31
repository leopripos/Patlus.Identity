using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public UpdateCommandHandler(IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;

        }
        public async Task<Pool> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));
            if (!request.HasName && !request.HasDescription) throw new ArgumentException($"One of {nameof(request.Name)} or {nameof(request.Description)} should be filled.");

            var currentTime = _timeService.Now;

            var entity = await _dbService.Pools.Where(e => e.Id == request.Id).SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), new { request.Id });
            }

            var valueChanges = new Dictionary<string, DeltaValue>();

            if (request.HasName)
            {
                valueChanges.Add(nameof(request.Name), new DeltaValue(entity.Name, request.Name!));
                entity.Name = request.Name!;
            }

            if (request.HasDescription)
            {
                valueChanges.Add(nameof(request.Description), new DeltaValue(entity.Description, request.Description!));
                entity.Description = request.Description!;
            }

            var notification = new UpdatedNotification(
                entity,
                valueChanges,
                request.RequestorId.Value,
                currentTime
            );

            entity.LastModifiedTime = currentTime;

            _dbService.Update(entity);

            await _dbService.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(notification, cancellationToken);

            return entity;
        }
    }
}
