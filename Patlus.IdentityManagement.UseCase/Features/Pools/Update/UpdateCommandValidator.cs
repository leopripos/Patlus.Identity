using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>, IFeatureValidator<UpdateCommand>
    {
        private readonly IMasterDbContext dbService;

        public UpdateCommandValidator(IMasterDbContext dbService)
        {
            this.dbService = dbService;

            RuleFor(r => r.Name)
                .NotEmpty()
                .MustAsync(UniqueName);

            RuleFor(r => r.Description)
                .NotEmpty();

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }

        private async Task<bool> UniqueName(string name, CancellationToken cancellationToken)
        {
            var count = await dbService.Pools.Where(e => e.Name == name).CountAsync(cancellationToken);

            return count == 0;
        }
    }
}
