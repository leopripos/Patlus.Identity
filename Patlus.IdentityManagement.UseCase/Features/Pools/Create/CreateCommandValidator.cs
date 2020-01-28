using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>, IFeatureValidator<CreateCommand>
    {
        private readonly IMasterDbContext dbService;

        public CreateCommandValidator(IMasterDbContext dbService)
        {
            this.dbService = dbService;

            RuleFor(r => r.Active)
                .NotEmpty();

            RuleFor(r => r.Name)
                .NotEmpty()
                .MustAsync(UniqueName);

            RuleFor(r => r.Description)
                .NotEmpty();

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }

        private async Task<bool> UniqueName(string value, CancellationToken cancellationToken)
        {
            var count = await dbService.Pools.Where(e => e.Name == value).CountAsync(cancellationToken).ConfigureAwait(false);

            return count == 0;
        }
    }
}
