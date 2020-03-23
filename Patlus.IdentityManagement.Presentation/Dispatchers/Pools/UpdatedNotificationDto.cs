using Patlus.Common.UseCase.Notifications;
using System;
using System.Collections.Generic;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Pools
{
    public class UpdatedNotificationDto
    {
        public Guid Id { get; set; }
        public Dictionary<string, ValueChanged> Values { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
