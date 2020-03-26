using MediatR;

namespace Patlus.Common.UseCase
{
    public interface IQueryFeatureHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IQueryFeature<TResult>
    { }
}
