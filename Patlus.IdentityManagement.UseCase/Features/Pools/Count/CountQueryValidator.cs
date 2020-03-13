using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Count
{
    public class CountQueryValidator : AbstractValidator<CountQuery>, IFeatureValidator<CountQuery>
    {
        public CountQueryValidator()
        {
            RuleFor(r => r.RequestorId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
