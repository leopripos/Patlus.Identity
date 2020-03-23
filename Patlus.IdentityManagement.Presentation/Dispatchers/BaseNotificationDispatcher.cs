using AutoMapper;
using MediatR;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.EventDispatcher.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Presentation.Dispatchers
{
    public abstract class BaseNotificationDispatcher<TNotification, TNotificationDto> : INotificationHandler<TNotification>
        where TNotification : IFeatureNotification
    {
        protected readonly IEventDispatcher Dispatcher;
        protected readonly string Topic;
        protected readonly IMapper Mapper;

        protected BaseNotificationDispatcher(string topic, IEventDispatcher dispatcher, IMapper mapper)
        {
            Topic = topic;
            Dispatcher = dispatcher;
            Mapper = mapper;
        }

        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            return Dispatcher.Dispatch(
                topic: Topic,
                notification: Mapper.Map<TNotificationDto>(notification), 
                orderGroup: notification.OrderingGroup
            );
        }
    }
}
