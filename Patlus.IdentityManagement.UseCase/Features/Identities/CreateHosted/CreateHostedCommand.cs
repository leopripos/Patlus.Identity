using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreateHostedCommand : ICommandFeature<Identity>
    {
        public Guid? PoolId { get; set; }
        public string Name { get; set; } = null!;
        public string AccountName { get; set; } = null!;
        public string AccountPassword { get; set; } = null!;
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
