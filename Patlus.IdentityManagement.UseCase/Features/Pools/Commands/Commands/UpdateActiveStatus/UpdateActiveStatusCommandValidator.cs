using FluentValidation;
using Patlus.Common.UseCase;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.Commands.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandValidator : AbstractValidator<UpdateActiveStatusCommand>, IFeatureValidator<UpdateActiveStatusCommand>
    {
        public UpdateActiveStatusCommandValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty();

            RuleFor(r => r.Active)
                .NotEmpty();

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }
    }
}
