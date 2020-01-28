using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreatedNotification : BaseCreatedNotification<Identity>
    {
        public CreatedNotification(Identity entity, Guid by, DateTimeOffset time)
            : base(entity, by, time)
        { }
    }
}
