using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Tokens
{
    public class RefreshedNotificationDispatcher
        : BaseNotificationDispatcher<RefreshedNotification, RefreshedNotificationDto>
    {
        public RefreshedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Tokens, dispatcher, mapper)
        { }
    }
}
