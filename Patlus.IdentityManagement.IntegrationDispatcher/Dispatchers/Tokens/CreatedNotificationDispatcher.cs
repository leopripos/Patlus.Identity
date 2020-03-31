using AutoMapper;
using Patlus.Common.Presentation.Services;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Tokens
{
    public class CreatedNotificationDispatcher : IFeatureNotificationHandler<CreatedNotification>
    {
        private readonly IEventDispatcher _dispatcher;
        private readonly IMapper _mapper;

        public CreatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
        {
            _dispatcher = dispatcher;
            _mapper = mapper;
        }

        public Task Handle(CreatedNotification notification, CancellationToken cancellationToken)
        {
            return _dispatcher.DispatchAsync(
                topic: Topics.Tokens,
                notification: _mapper.Map<CreatedNotificationDto>(notification),
                orderGroup: notification.OrderingGroup,
                cancellationToken: cancellationToken
            );
        }
    }
}
