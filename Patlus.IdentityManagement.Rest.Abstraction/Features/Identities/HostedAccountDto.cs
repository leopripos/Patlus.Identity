using Patlus.Common.Presentation;
using System;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    public class HostedAccountDto : IDto
    {
        public string Name { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
    }
}
