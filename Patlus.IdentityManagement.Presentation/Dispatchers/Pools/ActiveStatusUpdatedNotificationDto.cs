using System;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class ActiveStatusUpdatedNotificationDto
    {
        public bool Id { get; set; }
        public bool OldVaiue { get; set; }
        public bool NewValue { get; set; }
        public Guid By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
