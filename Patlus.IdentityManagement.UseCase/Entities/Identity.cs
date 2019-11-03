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
        public string Name { get; set; }

        public Guid CreatorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public bool Archived { get; set; }

        public Pool Pool { get; set; }
        public HostedAccount HostedAccount { get; set; }
    }
}
