using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Create
{
    public class CreateCommand : ICommandFeature<Pool>
    {
        public bool? Active { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
