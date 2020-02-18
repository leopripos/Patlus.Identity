using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetAll
{
    public class GetAllQueryValidator : AbstractValidator<GetAllQuery>, IFeatureValidator<GetAllQuery>
    {
        public GetAllQueryValidator()
        {
            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
