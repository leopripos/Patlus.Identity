using Patlus.Common.UseCase.Queries.Count;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Count
{
    public class CountQueryHandler : BaseCountQueryHandler<CountQuery, Pool>
    {
        public CountQueryHandler(IMasterDbContext dbService) : base(dbService.Pools)
        { }
    }
}
