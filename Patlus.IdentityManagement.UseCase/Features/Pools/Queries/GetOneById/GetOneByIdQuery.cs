using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Queries.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<IQueryable<Pool>>
    {
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
