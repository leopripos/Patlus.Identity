using Patlus.Common.UseCase.Queries.Exist;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.Exist
{
    public class ExistQueryHandler : BaseExistQueryHandler<ExistQuery, Identity>
    {
        public ExistQueryHandler(IMasterDbContext dbService) : base(dbService.Identities)
        { }
    }
}
