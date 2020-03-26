using Patlus.Common.Presentation;
using Patlus.Common.Presentation.Notifications;
using System;
using System.Collections.Generic;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class UpdatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Dictionary<string, DeltaValueDto> Values { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
