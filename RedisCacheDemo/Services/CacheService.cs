using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace RedisCacheDemo.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public CacheService(IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = cache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> Get(string key)
        {
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Set(string key, object value, TimeSpan time)
        {
            string data = JsonConvert.SerializeObject(value, new JsonSerializerSettings(){
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await _cache.SetStringAsync(key, data, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = time });
        }

        public async Task Remove(string pattern)
        {
            foreach (string key in GetKeyFromPattern(pattern+"*"))
            {
                await _cache.RemoveAsync(key);
            }
        }

        private IEnumerable<string> GetKeyFromPattern(string pattern)
        {
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key;
                }
            }
        }
    }
}
