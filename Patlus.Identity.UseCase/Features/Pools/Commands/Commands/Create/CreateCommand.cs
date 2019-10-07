using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;

namespace Patlus.Identity.UseCase.Features.Pools.Commands.Create
{
    public class CreateCommand : ICommandFeature<Pool>
    {
        public Guid Id { get; set; }
        public bool? Active { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
