using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using Patlus.IdentityManagement.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Queries.GetAll
{
    public class GetAllQueryHandler : IQueryFeatureHandler<GetAllQuery, IQueryable<Account>>
    {
        private readonly IMasterDbContext dbService;

        public GetAllQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<IQueryable<Account>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Accounts;

            return Task.FromResult(query);
        }
    }
}
