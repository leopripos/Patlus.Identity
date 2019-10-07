using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.Identity.UseCase.Features.Pools.Queries.GetOneById
{
    public class GetOneByIdQuery : IQueryFeature<IQueryable<Pool>>
    {
        public Guid? Id { get; set; }
        public Guid? RequestorId { get; set; }
    }
}
