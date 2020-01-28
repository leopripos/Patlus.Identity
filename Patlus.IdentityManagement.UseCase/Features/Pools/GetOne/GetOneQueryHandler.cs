using Patlus.Common.UseCase.Queries.GetOne;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetOne
{
    public class GetOneQueryHandler : BaseGetOneQueryHandler<GetOneQuery, Pool>
    {
        public GetOneQueryHandler(IMasterDbContext dbService) : base(dbService.Pools)
        { }
    }
}
