using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.Common.UseCase.Validators;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Update
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>, IFeatureValidator<UpdateCommand>
    {
        public UpdateCommandValidator(IMasterDbContext dbService)
        {
            RuleFor(r => r.Id)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.Name)
                .NotEmpty().Unless(e => !e.HasDescription).WithErrorCode(ValidationErrorCodes.NotEmpty)
                .Unique(dbService.Pools, (value) => (x => x.Name == value)).WithErrorCode(ValidationErrorCodes.Unique);

            RuleFor(r => r.Description)
                .NotEmpty().Unless(e => !e.HasName).WithErrorCode(ValidationErrorCodes.NotEmpty);

            RuleFor(r => r.RequestorId)
                .NotEmpty().WithErrorCode(ValidationErrorCodes.NotEmpty);
        }
    }
}
