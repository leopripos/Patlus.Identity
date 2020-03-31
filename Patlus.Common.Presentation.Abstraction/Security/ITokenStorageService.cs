using System;
using System.Threading.Tasks;

namespace Patlus.Common.Presentation.Security
{
    public interface ITokenStorageService
    {
        bool HasToken(Guid tokenId, Guid authKey);
        Task Set(Guid tokenId, Guid authKey, DateTimeOffset expiredTime);
        Task Remove(Guid tokenId);
    }
}
