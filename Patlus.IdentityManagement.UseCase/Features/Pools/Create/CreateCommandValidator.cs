using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Create
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>, IFeatureValidator<CreateCommand>
    {
        public CreateCommandValidator(IMasterDbContext dbService)
        {
            RuleFor(r => r.Active)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.Name)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty)
                .MinimumLength(4).WithErrorCode(ValidationErrorCodes.MinLength)
                .Matches(@"^[a-zA-Z]").WithErrorCode(ValidationErrorCodes.PatternMatch)
                .Unique(dbService.Pools, (value) => (x => x.Name == value)).WithErrorCode(ValidationErrorCodes.Unique);

            RuleFor(r => r.Description)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
