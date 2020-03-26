using Patlus.Common.Presentation;
using System;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Identities
{
    public class HostedCreatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
