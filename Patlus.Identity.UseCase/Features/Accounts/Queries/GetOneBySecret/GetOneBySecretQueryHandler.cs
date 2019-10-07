using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Identity.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetOneBySecret
{
    public class GetOneBySecretQueryHandler : IQueryFeatureHandler<GetOneBySecretQuery, Account>
    {
        private readonly IMasterDbContext dbService;
        private readonly IPasswordService passwordService;

        public GetOneBySecretQueryHandler(IMasterDbContext dbService, IPasswordService passwordService)
        {
            this.dbService = dbService;
            this.passwordService = passwordService;
        }

        public Task<Account> Handle(GetOneBySecretQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Accounts.Include(e => e.Hosted).Where(e => e.Hosted.Name == request.Name);

            var entity = query.FirstOrDefault();

            if (entity == null || !passwordService.ValidatePasswordHash(entity.Hosted.Password, request.Password))
            {
                throw new NotFoundException(nameof(Account), new { request.Name, request.Password });
            }

            return Task.FromResult(entity);
        }
    }
}
