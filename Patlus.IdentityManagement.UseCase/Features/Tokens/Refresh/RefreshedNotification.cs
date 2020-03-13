using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh
{
    public class RefreshedNotification : BaseCreatedNotification<Token>
    {
        public RefreshedNotification(Token entity, Guid? by, DateTimeOffset time)
            : base(entity, by, time)
        { }
    }
}
