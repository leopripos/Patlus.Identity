using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Rest.Services
{
    public interface IPoolResolver
    {
        Pool Current { get; }
    }
}
