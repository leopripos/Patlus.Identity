using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<Pool>
    {
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
