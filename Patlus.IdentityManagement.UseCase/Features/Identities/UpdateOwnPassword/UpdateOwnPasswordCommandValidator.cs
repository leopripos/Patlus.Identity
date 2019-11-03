using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommandValidator : AbstractValidator<UpdateOwnPasswordCommand>, IFeatureValidator<UpdateOwnPasswordCommand>
    {
        private readonly IMasterDbContext dbService;
        private readonly IPasswordService passwordService;

        public UpdateOwnPasswordCommandValidator(IMasterDbContext dbService, IPasswordService passwordService)
        {
            this.dbService = dbService;
            this.passwordService = passwordService;

            RuleFor(r => r.RequestorId)
                .NotEmpty();

            RuleFor(r => r.OldPassword)
                .NotEmpty()
                .MustAsync(CorrectOldPassword).WithMessage(e => $"Invalid { nameof(e.OldPassword) } password");

            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(10);

            RuleFor(r => r.RetypeNewPassword)
                .NotEmpty()
                .MustAsync(UqualToNewPassword).WithMessage(e => $"{ nameof(e.RetypeNewPassword) } should equal to { nameof(e.NewPassword) } ");
        }

        private Task<bool> CorrectOldPassword(UpdateOwnPasswordCommand command, string oldPassword, CancellationToken cancellationToken)
        {
            var entity = dbService.HostedAccounts.Where(e => e.Id == command.RequestorId).FirstOrDefault();

            if (entity == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(passwordService.ValidatePasswordHash(entity.Password, oldPassword));
        }

        private Task<bool> UqualToNewPassword(UpdateOwnPasswordCommand command, string retypeNewPassword, CancellationToken cancellationToken)
        {
            return Task.FromResult(retypeNewPassword == command.NewPassword);
        }
    }
}
