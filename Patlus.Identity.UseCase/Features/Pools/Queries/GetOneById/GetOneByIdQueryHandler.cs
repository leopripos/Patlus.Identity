using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Entities;
using Patlus.Identity.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Pools.Queries.GetOneById
{
    public class GetOneByIdQueryHandler : IQueryFeatureHandler<GetOneByIdQuery, IQueryable<Pool>>
    {
        private readonly IMetadataDbContext dbService;

        public GetOneByIdQueryHandler(IMetadataDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<IQueryable<Pool>> Handle(GetOneByIdQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Pools.Where(e => e.Id == request.Id);

            return Task.FromResult(query);
        }
    }
}
