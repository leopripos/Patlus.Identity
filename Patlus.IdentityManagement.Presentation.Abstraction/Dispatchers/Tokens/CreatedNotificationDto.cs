using Patlus.Common.Presentation;
using System;

namespace Patlus.IdentityManagement.Presentation.Dispatchers.Tokens
{
    public class CreatedNotificationDto : IDto
    {
        public Guid Id { get; set; }
        public Guid IdentityId { get; set; }
        public string Scheme { get; set; }
        public string Access { get; set; }
        public string Refresh { get; set; }
        public Guid? By { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
