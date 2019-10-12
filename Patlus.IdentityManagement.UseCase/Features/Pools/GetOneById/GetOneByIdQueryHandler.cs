using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetOneById
{
    public class GetOneByIdQueryHandler : IQueryFeatureHandler<GetOneByIdQuery, Pool>
    {
        private readonly IMasterDbContext dbService;

        public GetOneByIdQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<Pool> Handle(GetOneByIdQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Pools.Where(e => e.Id == request.Id);

            var entity = query.FirstOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Pool), request.Id); ;
            }

            return Task.FromResult(entity);
        }
    }
}
