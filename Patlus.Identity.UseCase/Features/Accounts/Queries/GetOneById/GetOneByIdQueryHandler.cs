using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Exceptions;
using Patlus.Identity.UseCase.Services;
using Patlus.Identity.UseCase.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Accounts.Queries.GetOneById
{
    public class GetOneByIdQueryHandler : IQueryFeatureHandler<GetOneByIdQuery, Account>
    {
        private readonly IMasterDbContext dbService;

        public GetOneByIdQueryHandler(IMasterDbContext dbService)
        {
            this.dbService = dbService;
        }

        public Task<Account> Handle(GetOneByIdQuery request, CancellationToken cancellationToken)
        {
            var query = dbService.Accounts.Where(e => e.Id == request.Id);

            var entity = query.FirstOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Account), request.Id);
            }

            return Task.FromResult(entity);
        }
    }
}
