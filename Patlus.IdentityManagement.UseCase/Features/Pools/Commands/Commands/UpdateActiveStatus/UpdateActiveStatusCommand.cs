using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Commands.UpdateActiveStatus
{
    public class UpdateActiveStatusCommand : ICommandFeature<Pool>
    {
        public Guid? Id { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
