using Microsoft.Extensions.Caching.Distributed;
using Patlus.Common.Presentation.Security;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Infrastructure.Cache.Token
{
    public class DistributedTokenStorageService : ITokenStorageService
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedTokenStorageService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public bool HasToken(Guid tokenId, Guid authKey)
        {
            var key = $"{tokenId}";
            var value = _distributedCache.GetString(key);

            return value == authKey.ToString();
        }

        public Task Set(Guid tokenId, Guid authKey, DateTimeOffset expiredTime)
        {
            var key = $"{tokenId}";

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiredTime
            };

            return _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(authKey.ToString()), options);
        }

        public Task Remove(Guid tokenId)
        {
            var key = $"{tokenId}";

            return _distributedCache.RemoveAsync(key);
        }
    }
}
