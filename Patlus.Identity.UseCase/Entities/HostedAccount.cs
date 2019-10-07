using Patlus.Common.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Entities
{
    public class HostedAccount : IStandardEntity, IAccount
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Guid CreatorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public bool Archived { get; set; }

        public Account Account { get; set; }
    }
}
