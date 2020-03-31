using Patlus.Common.Presentation.Responses;
using System;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Pools
{
    public class ActiveStatusUpdatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public bool OldVaiue { get; set; }
        public bool NewValue { get; set; }
        public Guid By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
