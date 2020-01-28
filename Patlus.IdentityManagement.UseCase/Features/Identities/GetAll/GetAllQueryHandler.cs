using Patlus.Common.UseCase.Queries.GetAll;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetAll
{
    public class GetAllQueryHandler : BaseGetAllQueryHandler<GetAllQuery, Identity>
    {
        public GetAllQueryHandler(IMasterDbContext dbService) : base(dbService.Identities)
        { }
    }
}
