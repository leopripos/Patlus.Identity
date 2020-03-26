using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class HostedCreatedNotificationDispatcher : IFeatureNotificationHandler<HostedCreatedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public HostedCreatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(HostedCreatedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Identities,
                notification: _mapper.Map<HostedCreatedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
