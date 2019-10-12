using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted
{
    public class CreateHostedCommandValidator : AbstractValidator<CreateHostedCommand>, IFeatureValidator<CreateHostedCommand>
    {
        private readonly IMasterDbContext dbService;

        public CreateHostedCommandValidator(IMasterDbContext dbService)
        {
            this.dbService = dbService;

            RuleFor(r => r.PoolId)
                .MustAsync(PoolExsist);

            RuleFor(r => r.Active)
                .NotEmpty();

            RuleFor(r => r.Name)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(10)
                .Matches(@"^[a-zA-Z][a-zA-Z0-9]*")
                .MustAsync(UniqueName);

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(10);

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }

        private Task<bool> PoolExsist(Guid? id, CancellationToken cancellationToken)
        {
            var count = dbService.Pools.Where(e => e.Id == id).Count();

            return Task.FromResult(count == 0);
        }

        public Task<bool> UniqueName(string name, CancellationToken cancellationToken)
        {
            var count = dbService.Identities.Where(e => e.HostedAccount.Name == name).Count();

            return Task.FromResult(count == 0);
        }
    }
}
