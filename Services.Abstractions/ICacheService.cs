using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cachekey);

        Task SetAsync(string cachekey, object value, TimeSpan timeToLive);
    }
}
