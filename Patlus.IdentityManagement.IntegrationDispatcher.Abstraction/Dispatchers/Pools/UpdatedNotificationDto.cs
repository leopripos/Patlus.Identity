using Patlus.Common.Presentation.Responses;
using Patlus.Common.Presentation.Responses.Notifications;
using System;
using System.Collections.Generic;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Pools
{
    public class UpdatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Dictionary<string, DeltaValueDto> Values { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
