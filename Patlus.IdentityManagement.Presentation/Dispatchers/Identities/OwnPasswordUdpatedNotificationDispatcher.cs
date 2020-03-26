using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class OwnPasswordUdpatedNotificationDispatcher : IFeatureNotificationHandler<OwnPasswordUdpatedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public OwnPasswordUdpatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(OwnPasswordUdpatedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Identities,
                notification: _mapper.Map<OwnPasswordUdpatedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
