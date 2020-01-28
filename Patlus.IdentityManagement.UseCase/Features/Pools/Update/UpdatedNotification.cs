using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Collections.Generic;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdatedNotification : BaseUpdatedNotification<Pool>
    {
        public UpdatedNotification(Pool entity, Dictionary<string, ValueChanged> values, Guid by, DateTimeOffset time)
            : base(entity, values, by, time)
        { }
    }
}
