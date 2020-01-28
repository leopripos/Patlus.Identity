using Patlus.Common.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Entities
{
    public class Identity : IStandardEntity
    {
        public Guid Id { get; set; }
        public Guid PoolId { get; set; }
        public Guid AuthKey { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; } = null!;

        public Guid CreatorId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }

        public bool Archived { get; set; }

        public Pool? Pool { get; set; }
        public HostedAccount? HostedAccount { get; set; }
    }
}
