using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword
{
    public class UpdateOwnPasswordCommandValidator : AbstractValidator<UpdateOwnPasswordCommand>, IFeatureValidator<UpdateOwnPasswordCommand>
    {
        private readonly IMasterDbContext _dbService;
        private readonly IPasswordService _passwordService;

        public UpdateOwnPasswordCommandValidator(IMasterDbContext dbService, IPasswordService passwordService)
        {
            _dbService = dbService;
            _passwordService = passwordService;

            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.OldPassword)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty)
                .MustAsync(CorrectOldPassword).WithMessage(e => $"Invalid { nameof(e.OldPassword) } password").WithErrorCode(ValidationErrorCodes.Invalid);

            RuleFor(r => r.NewPassword)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty)
                .MinimumLength(6).WithErrorCode(ValidationErrorCodes.MinLength)
                .MaximumLength(20).WithErrorCode(ValidationErrorCodes.MaxLength);

            RuleFor(r => r.RetypeNewPassword)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty)
                .Equal(e => e.NewPassword).WithErrorCode(ValidationErrorCodes.Equal);
        }

        private Task<bool> CorrectOldPassword(UpdateOwnPasswordCommand command, string oldPassword, CancellationToken cancellationToken)
        {
            var entity = _dbService.Identities
                .Include(e => e.HostedAccount)
                .Where(e => e.Id == command.RequestorId && e.Archived == false).FirstOrDefault();

            if (entity is null || entity.HostedAccount is null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_passwordService.ValidatePasswordHash(entity.HostedAccount.Password, oldPassword));
        }
    }
}
