using Patlus.Common.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Entities
{
    public class HostedAccount : IStandardEntity, IAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public Guid CreatorId { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastModifiedTime { get; set; }

        public bool Archived { get; set; }

        public Identity Identity { get; set; } = null!;
    }
}
