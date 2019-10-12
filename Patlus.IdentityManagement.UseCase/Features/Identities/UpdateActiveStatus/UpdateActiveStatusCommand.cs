using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus
{
    public class UpdateActiveStatusCommand : ICommandFeature<Identity>
    {
        public Guid? PoolId { get; set; }
        public Guid? Id { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
