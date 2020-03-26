using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class HostedCreatedNotification : BaseCreatedNotification<Identity>
    {
        public HostedCreatedNotification(Identity entity, Guid by, DateTimeOffset time)
            : base(entity, by, time)
        { }
    }
}
