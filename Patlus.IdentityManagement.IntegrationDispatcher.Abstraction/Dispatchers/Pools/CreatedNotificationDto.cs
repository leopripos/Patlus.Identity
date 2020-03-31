using Patlus.Common.Presentation.Responses;
using System;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Pools
{
    public class CreatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
