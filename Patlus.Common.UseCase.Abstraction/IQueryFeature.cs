using MediatR;

namespace Patlus.Common.UseCase
{
    public interface IQueryFeature<out TResponse> : IRequest<TResponse>, IFeature
    { }
}
