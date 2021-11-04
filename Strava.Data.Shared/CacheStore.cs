using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava.Data.Shared
{
    public class CacheStore
    {
        public static IMemoryCache Cache;
        private static List<string> keys = new List<string>();
        private static TimeSpan _expiry = TimeSpan.FromHours(1);

        private static string cKey<T>(string key) => $"{nameof(T)}.{key}";

        public static Task Set<T>(string key, T obj)
        {
            if (Cache == null)
                return Task.CompletedTask;

            key = cKey<T>(key);
            if (!keys.Contains(key))
                keys.Add(key);

            Cache.Set(key, obj, _expiry);

            return Task.CompletedTask;
        }

        public static Task<T> Get<T>(string key)
        {
            if (Cache == null)
                return Task.FromResult(default(T));

            T item = Cache.Get<T>(cKey<T>(key)) ?? default;

            return Task.FromResult(item);
        }
        
    }
}
