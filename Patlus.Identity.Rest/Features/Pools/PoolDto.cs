using Patlus.Common.UseCase;
using System;

namespace Patlus.Identity.Rest.Features.Pools
{
    public class PoolDto : IDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
