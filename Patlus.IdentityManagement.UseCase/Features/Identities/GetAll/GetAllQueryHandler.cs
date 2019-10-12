using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetAll
{
    public class GetAllQueryHandler : IQueryFeatureHandler<GetAllQuery, Identity[]>
    {
        private readonly IMasterDbContext dbService;

        public GetAllQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<Identity[]> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Identities;

            if (request.PoolId.HasValue)
            {
                query = query.Where(e => e.PoolId == request.PoolId);
            }

            query = query.Include(e => e.HostedAccount);

            return Task.FromResult(query.ToArray());
        }
    }
}
