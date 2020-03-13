using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandValidator : AbstractValidator<UpdateActiveStatusCommand>, IFeatureValidator<UpdateActiveStatusCommand>
    {
        public UpdateActiveStatusCommandValidator()
        {
            RuleFor(r => r.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.Active)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.RequestorId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
