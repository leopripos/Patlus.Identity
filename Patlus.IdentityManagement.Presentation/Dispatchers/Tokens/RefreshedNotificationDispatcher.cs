using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Tokens
{
    public class RefreshedNotificationDispatcher : IFeatureNotificationHandler<RefreshedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public RefreshedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(RefreshedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Tokens,
                notification: _mapper.Map<RefreshedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
