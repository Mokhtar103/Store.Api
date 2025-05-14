using Domain.Contract;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class CacheService(ICacheRepository cacheRepository) : ICacheService
    {
        public Task<string?> GetAsync(string cachekey)
            => cacheRepository.GetAsync(cachekey);

        public async Task SetAsync(string cachekey, object value, TimeSpan timeToLive)
        {
            var seriliazedObj = JsonSerializer.Serialize(value);

            await cacheRepository.SetAsync(cachekey, seriliazedObj, timeToLive);
        }
    }
}
