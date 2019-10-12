using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneById
{
    public class GetOneByIdQueryHandler : IQueryFeatureHandler<GetOneByIdQuery, Identity>
    {
        private readonly IMasterDbContext dbService;

        public GetOneByIdQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<Identity> Handle(GetOneByIdQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Identities.Where(e => e.Id == request.Id);

            if (request.PoolId.HasValue)
            {
                query = query.Where(e => e.PoolId == request.PoolId);
            }

            var entity = query.FirstOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Identity), request.Id);
            }

            return Task.FromResult(entity);
        }
    }
}
