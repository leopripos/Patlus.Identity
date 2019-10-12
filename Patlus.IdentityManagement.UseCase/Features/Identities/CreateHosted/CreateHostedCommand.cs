using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreateHostedCommand : ICommandFeature<Identity>
    {
        public Guid? PoolId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
