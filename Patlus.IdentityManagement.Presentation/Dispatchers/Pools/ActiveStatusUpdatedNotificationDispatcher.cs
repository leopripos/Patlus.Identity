using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class ActiveStatusUpdatedNotificationDispatcher
        : BaseNotificationDispatcher<ActiveStatusUpdatedNotification, ActiveStatusUpdatedNotificationDto>
    {
        public ActiveStatusUpdatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Pools, dispatcher, mapper)
        { }
    }
}
