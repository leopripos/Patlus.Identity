using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class ActiveStatusUpdatedNotificationDispatcher 
        : BaseNotificationDispatcher<ActiveStatusUpdatedNotification, ActiveStatusUpdatedNotificationDto>
    {
        public ActiveStatusUpdatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Identities, dispatcher, mapper)
        {}
    }
}
