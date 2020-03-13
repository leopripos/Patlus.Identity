using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh
{
    public class RefreshCommandValidator : AbstractValidator<RefreshCommand>, IFeatureValidator<RefreshCommand>
    {
        public RefreshCommandValidator()
        {
            RuleFor(r => r.RefreshToken)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
