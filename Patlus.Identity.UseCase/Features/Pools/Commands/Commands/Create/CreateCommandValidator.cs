using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Identity.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.Identity.UseCase.Features.Pools.Commands.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>, IFeatureValidator<CreateCommand>
    {
        private readonly IMetadataDbContext dbService;

        public CreateCommandValidator(IMetadataDbContext dbService)
        {
            this.dbService = dbService;

            RuleFor(r => r.Id)
                .NotEmpty()
                .MustAsync(UniqueId);

            RuleFor(r => r.Active)
                .NotEmpty();

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }

        public Task<bool> UniqueId(CreateCommand command, Guid value, CancellationToken cancellationToken)
        {
            var count = dbService.Pools.Where(e => e.Id == value).Count();

            return Task.FromResult(count == 0);
        }
    }
}
