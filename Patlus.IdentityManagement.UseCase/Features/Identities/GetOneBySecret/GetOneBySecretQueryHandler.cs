using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.IdentityManagement.UseCase.Entities;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret
{
    public class GetOneBySecretQueryHandler : IQueryFeatureHandler<GetOneBySecretQuery, Identity>
    {
        private readonly IMasterDbContext _dbService;
        private readonly IPasswordService _passwordService;

        public GetOneBySecretQueryHandler(IMasterDbContext dbService, IPasswordService passwordService)
        {
            _dbService = dbService;
            _passwordService = passwordService;
        }

        public Task<Identity> Handle(GetOneBySecretQuery request, CancellationToken cancellationToken)
        {
            if (request.PoolId is null) throw new ArgumentNullException(nameof(request.PoolId));
            if (request.Name is null) throw new ArgumentNullException(nameof(request.Name));
            if (request.Password is null) throw new ArgumentNullException(nameof(request.Password));

            var query = _dbService.Identities
                .Include(e => e.Pool)
                .Include(e => e.HostedAccount)
                .Where(
                    e => e.PoolId == request.PoolId && e.Active == true && e.Archived == false
                    && e.Pool != null && e.Pool.Active == true && e.Pool.Archived == false
                    && e.HostedAccount != null && e.HostedAccount.Archived == false && e.HostedAccount.Name == request.Name
                );

            var entity = query.FirstOrDefault();

            if (entity is null || entity.HostedAccount is null || !_passwordService.ValidatePasswordHash(entity.HostedAccount.Password, request.Password))
            {
                throw new NotFoundException(nameof(Identity), new { request.PoolId, request.Name, request.Password });
            }

            return Task.FromResult(entity);
        }
    }
}
