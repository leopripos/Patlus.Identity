using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Pools
{
    public class ActiveStatusUpdatedNotificationDispatcher : IFeatureNotificationHandler<ActiveStatusUpdatedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public ActiveStatusUpdatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(ActiveStatusUpdatedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Pools,
                notification: _mapper.Map<ActiveStatusUpdatedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
