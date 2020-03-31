using MediatR;

namespace Patlus.Common.UseCase
{
    public interface ICommandFeature<out TResponse> : IRequest<TResponse>, IFeature
    { }
}
