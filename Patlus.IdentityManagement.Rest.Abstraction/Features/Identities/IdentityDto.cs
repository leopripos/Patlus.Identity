using Patlus.Common.Presentation;
using System;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    public class IdentityDto : IDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;
        public Guid CreatorId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }

        public HostedAccountDto? HostedAccount { get; set; }
    }
}
