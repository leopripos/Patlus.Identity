using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus
{
    public class ActiveStatusUpdatedNotification : BaseActiveStatusUpdatedNotification<Pool>
    {
        public ActiveStatusUpdatedNotification(Pool entity, bool oldValue, bool newValue, Guid by, DateTimeOffset time)
                : base(entity, oldValue, newValue, by, time)
        { }
    }
}
