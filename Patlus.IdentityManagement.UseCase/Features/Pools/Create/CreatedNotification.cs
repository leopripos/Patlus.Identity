using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Create
{
    public class CreatedNotification : BaseCreatedNotification<Pool>
    {
        public CreatedNotification(Pool entity, Guid by, DateTimeOffset time)
            : base(entity, by, time)
        { }
    }
}
