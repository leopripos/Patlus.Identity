using Patlus.Common.UseCase.Queries.Count;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.Count
{
    public class CountQueryHandler : BaseCountQueryHandler<CountQuery, Identity>
    {
        public CountQueryHandler(IMasterDbContext dbService) : base(dbService.Identities)
        { }
    }
}
