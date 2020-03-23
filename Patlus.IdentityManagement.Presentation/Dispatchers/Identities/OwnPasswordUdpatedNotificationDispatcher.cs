using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class OwnPasswordUdpatedNotificationDispatcher
        : BaseNotificationDispatcher<OwnPasswordUdpatedNotification, OwnPasswordUdpatedNotificationDto>
    {
        public OwnPasswordUdpatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Identities, dispatcher, mapper)
        { }
    }
}
