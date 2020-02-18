using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandValidator : AbstractValidator<UpdateActiveStatusCommand>, IFeatureValidator<UpdateActiveStatusCommand>
    {
        public UpdateActiveStatusCommandValidator()
        {
            RuleFor(e => e.Id)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(e => e.Active)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(e => e.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
