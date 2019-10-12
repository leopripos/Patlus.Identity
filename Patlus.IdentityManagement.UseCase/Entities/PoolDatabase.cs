using Patlus.Common.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Entities
{
    public class PoolDatabase : IStandardEntity
    {
        public Guid Id { get; set; }
        public string ConnectionString { get; set; }

        public Guid CreatorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public bool Archived { get; set; }

        public Pool Pool { get; set; }
    }
}
