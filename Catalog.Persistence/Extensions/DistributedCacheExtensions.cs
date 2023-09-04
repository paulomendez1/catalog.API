using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Persistence.Extensions
{
    public static class DistributedCacheExtensions
    {
        private static readonly JsonSerializerSettings _serializerSettings = CreateSettings();
 
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key)
        {
            var json = await cache.GetStringAsync(key);
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }
        public static async Task SetObjectAsync(this IDistributedCache cache, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value, _serializerSettings);
            await cache.SetStringAsync(key, json);
        }
        
        public static async Task SetObjectAsync(this IDistributedCache cache, string key,
        object value, DistributedCacheEntryOptions options)
        {
            var json = JsonConvert.SerializeObject(value, _serializerSettings);
            await cache.SetStringAsync(key, json, options);
        }
        private static JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings();
        }
    }
}
