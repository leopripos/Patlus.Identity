using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Pools.Queries.GetList
{
    public class GetListQueryHandler : IQueryFeatureHandler<GetListQuery, IQueryable<Pool>>
    {
        private readonly IMetadataDbContext dbService;

        public GetListQueryHandler(IMetadataDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<IQueryable<Pool>> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Pools;

            return Task.FromResult(query);
        }
    }
}
