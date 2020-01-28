using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommand : ICommandFeature<Pool>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? RequestorId { get; set; }
    }
}
