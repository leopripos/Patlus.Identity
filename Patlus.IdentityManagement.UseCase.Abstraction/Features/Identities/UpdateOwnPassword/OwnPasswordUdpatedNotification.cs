using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Collections.Generic;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class OwnPasswordUdpatedNotification : BaseUpdatedNotification<Identity>
    {
        public OwnPasswordUdpatedNotification(Identity entity, Guid by, DateTimeOffset time)
            : base(entity, new Dictionary<string, DeltaValue> (), by, time)
        { }
    }
}
