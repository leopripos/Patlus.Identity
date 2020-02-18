using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Exist
{
    public class ExistQueryValidator : AbstractValidator<ExistQuery>, IFeatureValidator<ExistQuery>
    {
        public ExistQueryValidator()
        {
            RuleFor(r => r.Condition)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
