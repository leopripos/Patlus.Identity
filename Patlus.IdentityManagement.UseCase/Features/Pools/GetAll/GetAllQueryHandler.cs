using Patlus.Common.UseCase.Queries.GetAll;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetAll
{
    public class GetAllQueryHandler : BaseGetAllQueryHandler<GetAllQuery, Pool>
    {
        public GetAllQueryHandler(IMasterDbContext dbService) : base(dbService.Pools)
        { }
    }
}
