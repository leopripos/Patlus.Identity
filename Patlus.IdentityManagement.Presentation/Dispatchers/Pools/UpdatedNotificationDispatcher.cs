using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Pools.Update;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class UpdatedNotificationDispatcher
        : BaseNotificationDispatcher<UpdatedNotification, UpdatedNotificationDto>
    {
        public UpdatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Pools, dispatcher, mapper)
        { }
    }
}
