using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.UseCase.Services
{
    public interface IDbActivatorService {
        Task<IDbInfo> Create(Guid id);
    }
}
