using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetAll
{
    public class GetAllQueryHandler : IQueryFeatureHandler<GetAllQuery, Pool[]>
    {
        private readonly IMasterDbContext dbService;

        public GetAllQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<Pool[]> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Pools;

            return Task.FromResult(query.ToArray());
        }
    }
}
