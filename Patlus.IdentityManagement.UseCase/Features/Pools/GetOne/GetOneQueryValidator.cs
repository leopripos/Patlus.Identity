using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.GetOne
{
    public class GetOneQueryValidator : AbstractValidator<GetOneQuery>, IFeatureValidator<GetOneQuery>
    {
        public GetOneQueryValidator()
        {
            RuleFor(r => r.Condition)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
