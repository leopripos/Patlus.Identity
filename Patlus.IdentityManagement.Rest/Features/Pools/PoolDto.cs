using Patlus.Common.UseCase;
using System;

namespace Patlus.IdentityManagement.Rest.Features.Pools
{
    public class PoolDto : IDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CreatorId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}
