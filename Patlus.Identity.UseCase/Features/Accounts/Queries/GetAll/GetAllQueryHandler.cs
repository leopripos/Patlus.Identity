using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetAll
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
