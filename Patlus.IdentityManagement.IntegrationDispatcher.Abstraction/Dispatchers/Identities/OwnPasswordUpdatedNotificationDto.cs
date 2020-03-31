using Patlus.Common.Presentation.Responses;
using System;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Identities
{
    public class OwnPasswordUdpatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Guid By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
