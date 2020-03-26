using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandHandler : ICommandFeatureHandler<UpdateActiveStatusCommand, Identity>
    {
        private readonly IMasterDbContext _dbService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public UpdateActiveStatusCommandHandler(IMasterDbContext dbService, ITimeService timeService, IMediator mediator)
        {
            _dbService = dbService;
            _timeService = timeService;
            _mediator = mediator;
        }

        public async Task<Identity> Handle(UpdateActiveStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.Id is null) throw new ArgumentNullException(nameof(request.Id));
            if (request.Active is null) throw new ArgumentNullException(nameof(request.Active));
            if (request.RequestorId is null) throw new ArgumentNullException(nameof(request.RequestorId));

            var currentTime = _timeService.Now;

            var query = _dbService.Identities.Where(e => e.Id == request.Id);

            if (request.PoolId.HasValue)
            {
                query = query.Where(e => e.PoolId == request.PoolId);
            }

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Identity), new { request.PoolId, request.Id });
            }

            var notification = new ActiveStatusUpdatedNotification(entity, entity.Active, request.Active.Value, request.RequestorId.Value, currentTime);

            entity.Active = request.Active.Value;
            entity.LastModifiedTime = currentTime;

            _dbService.Update(entity);

            await _dbService.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(notification, cancellationToken);

            return entity;
        }
    }
}
