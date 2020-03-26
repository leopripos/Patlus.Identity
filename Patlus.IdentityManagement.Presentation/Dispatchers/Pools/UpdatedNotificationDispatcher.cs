using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class UpdatedNotificationDispatcher : IFeatureNotificationHandler<UpdatedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public UpdatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(UpdatedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Pools,
                notification: _mapper.Map<UpdatedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
