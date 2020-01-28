using Patlus.Common.UseCase.Queries.GetOne;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOne
{
    public class GetOneQueryHandler : BaseGetOneQueryHandler<GetOneQuery, Identity>
    {
        public GetOneQueryHandler(IMasterDbContext dbService) : base(dbService.Identities)
        { }
    }
}
