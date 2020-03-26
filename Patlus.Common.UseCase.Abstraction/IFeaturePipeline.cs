using MediatR;

namespace Patlus.Common.UseCase.Behaviours
{
    public interface IFeaturePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
    }
}
