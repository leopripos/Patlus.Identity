using FluentValidation;

namespace Patlus.Common.UseCase
{
    public interface IFeatureValidator<TFeature> : IValidator<TFeature> where TFeature : IFeature
    { }
}
