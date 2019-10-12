using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Queries.GetOneById
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
