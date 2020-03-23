using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class HostedCreatedNotificationDispatcher
        : BaseNotificationDispatcher<HostedCreatedNotification, HostedCreatedNotificationDto>
    {
        public HostedCreatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Identities, dispatcher, mapper)
        { }
    }
}
