using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.Identity.UseCase.Features.Pools.Queries.GetList
{
    public class GetListQuery : IQueryFeature<IQueryable<Pool>>
    {
        public Guid? RequestorId { get; set; }
    }
}
