using Patlus.Common.UseCase.Notifications;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Create
{
    public class CreatedNotification : BaseCreatedNotification<Token>
    {
        public Guid IdentityId { get; }

        public override Guid OrderingGroup
        {
            get { return IdentityId; }
        }

        public CreatedNotification(Guid identityId, Token entity, Guid? by, DateTimeOffset time)
            : base(entity, by, time)
        {
            IdentityId = identityId;
        }
    }
}
