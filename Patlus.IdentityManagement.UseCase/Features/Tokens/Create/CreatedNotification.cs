using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Create
{
    public class CreatedNotification : BaseCreatedNotification<Token>
    {
        public CreatedNotification(Token entity, Guid? by, DateTimeOffset time)
            : base(entity, by, time)
        { }
    }
}
