using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Pools.Create;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class CreatedNotificationDispatcher
        : BaseNotificationDispatcher<CreatedNotification, CreatedNotificationDto>
    {
        public CreatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Pools, dispatcher, mapper)
        { }
    }
}
