using Patlus.Common.UseCase;
using System;

namespace Patlus.Identity.Rest.Features.Accounts
{
    public class AccountDto : IDto
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }

        public Guid CreatorId { get; set; }
        public DateTime CreatedTime { get; set; }

        public HostedAccountDto Hosted { get; set; }
    }

    public class HostedAccountDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
