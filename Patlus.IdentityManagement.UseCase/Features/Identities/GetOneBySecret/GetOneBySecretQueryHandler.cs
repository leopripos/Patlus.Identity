using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret
{
    public class GetOneBySecretQueryHandler : IQueryFeatureHandler<GetOneBySecretQuery, Identity>
    {
        private readonly IMasterDbContext dbService;
        private readonly IPasswordService passwordService;

        public GetOneBySecretQueryHandler(IMasterDbContext dbService, IPasswordService passwordService)
        {
            this.dbService = dbService;
            this.passwordService = passwordService;
        }

        public Task<Identity> Handle(GetOneBySecretQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Identities
                .Include(e => e.Pool)
                .Include(e => e.HostedAccount)
                .Where(
                    e => e.PoolId == request.PoolId 
                    &&
                    e.HostedAccount.Name == request.Name
                );

            var entity = query.FirstOrDefault();

            if (entity == null || !passwordService.ValidatePasswordHash(entity.HostedAccount.Password, request.Password))
            {
                throw new NotFoundException(nameof(Identity), new { request.Name, request.Password });
            }

            return Task.FromResult(entity);
        }
    }
}
