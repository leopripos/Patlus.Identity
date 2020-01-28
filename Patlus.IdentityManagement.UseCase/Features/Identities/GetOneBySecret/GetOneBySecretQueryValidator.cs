using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Identities.GetOneBySecret
{
    public class GetOneBySecretQueryValidator : AbstractValidator<GetOneBySecretQuery>, IFeatureValidator<GetOneBySecretQuery>
    {
        public GetOneBySecretQueryValidator(IMasterDbContext dbService)
        {

            RuleFor(r => r.PoolId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty)
                .Exist(dbService.Pools, (value) => x => x.Id == value && x.Active == true && x.Archived == false).WithErrorCode(ValidationErrorCodes.Exist);

            RuleFor(r => r.Name)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.Password)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
