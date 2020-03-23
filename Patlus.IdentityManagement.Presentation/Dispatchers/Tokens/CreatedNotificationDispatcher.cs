using AutoMapper;
using Patlus.IdentityManagement.EventDispatcher.Services;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Tokens
{
    public class CreatedNotificationDispatcher
        : BaseNotificationDispatcher<CreatedNotification, CreatedNotificationDto>
    {
        public CreatedNotificationDispatcher(IEventDispatcher dispatcher, IMapper mapper)
            : base(Topics.Tokens, dispatcher, mapper)
        { }
    }
}
