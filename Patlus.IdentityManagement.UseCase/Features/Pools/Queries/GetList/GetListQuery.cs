using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using System;
using System.Linq;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Queries.GetList
{
    public class GetListQuery : IQueryFeature<IQueryable<Pool>>
    {
        public Guid? RequestorId { get; set; }
    }
}
