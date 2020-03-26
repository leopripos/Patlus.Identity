using System;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Cache.Services
{
    public interface ITokenStorageService
    {
        bool HasToken(Guid tokenId, Guid authKey);
        Task Set(Guid tokenId, Guid authKey, DateTimeOffset expiredTime);
        Task Remove(Guid tokenId);
    }
}
