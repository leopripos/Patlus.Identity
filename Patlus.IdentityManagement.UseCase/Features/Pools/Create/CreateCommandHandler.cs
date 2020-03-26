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
        private readonly IMasterDbContext _dbService;
        private readonly IIdentifierService _identifierService;
        private readonly ITimeService _timeService;
        private readonly IMediator _mediator;

        public CreateCommandHandler(IMasterDbContext dbService, IIdentifierService identifierService, ITimeService timeService, IMediator mediator)
        {
            _dbService = dbService;
            _identifierService = identifierService;
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
                Id = _identifierService.NewGuid(),
                Active = request.Active.Value,
                Name = request.Name,
                Description = request.Description,
                CreatorId = request.RequestorId.Value,
                CreatedTime = currentTime,
                LastModifiedTime = currentTime,
            };

            _dbService.Add(entity);

            await _dbService.SaveChangesAsync(cancellationToken);

            var notification = new CreatedNotification(entity, request.RequestorId.Value, currentTime);

            await _mediator.Publish(notification, cancellationToken);

            return entity;
        }
    }
}
