using Patlus.Common.UseCase.Queries.Exist;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Exist
{
    public class ExistQueryHandler : BaseExistQueryHandler<ExistQuery, Pool>
    {
        public ExistQueryHandler(IMasterDbContext dbService) : base(dbService.Pools)
        { }
    }
}
