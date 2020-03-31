using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus
{
    public class ActiveStatusUpdatedNotification : BaseActiveStatusUpdatedNotification<Identity>
    {
        public ActiveStatusUpdatedNotification(Identity entity, bool oldValue, bool newValue, Guid by, DateTimeOffset time)
            : base(entity, oldValue, newValue, by, time)
        { }
    }
}
