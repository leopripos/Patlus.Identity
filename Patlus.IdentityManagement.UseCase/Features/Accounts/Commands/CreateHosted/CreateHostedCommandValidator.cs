using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Accounts.Commands.CreateHosted
{
    public class CreateHostedCommandValidator : AbstractValidator<CreateHostedCommand>, IFeatureValidator<CreateHostedCommand>
    {
        private readonly IMasterDbContext dbService;

        public CreateHostedCommandValidator(IMasterDbContext dbService)
        {
            this.dbService = dbService;

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

        public Task<bool> UniqueName(CreateHostedCommand command, string value, CancellationToken cancellationToken)
        {
            var count = dbService.Accounts.Where(e => e.Hosted.Name == value).Count();

            return Task.FromResult(count == 0);
        }
    }
}
