using FluentValidation;
using Patlus.Common.UseCase;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.UseCase.Features.Pools.UpdateActiveStatus
{
    public class UpdateActiveStatusCommandValidator : AbstractValidator<UpdateActiveStatusCommand>, IFeatureValidator<UpdateActiveStatusCommand>
    {
        private readonly IMasterDbContext dbService;

        public UpdateActiveStatusCommandValidator(IMasterDbContext dbService)
        {
            this.dbService = dbService;

            RuleFor(r => r.Active)
                .NotEmpty();

            RuleFor(r => r.RequestorId)
                .NotEmpty();
        }
    }
}
