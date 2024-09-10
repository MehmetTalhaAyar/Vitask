using Microsoft.Extensions.Caching.Memory;

namespace Vitask.Statics
{
    public static class CacheManager
    {
        private static IMemoryCache _memoryCache;

        private static Dictionary<string, List<string>> _cacheKeysByGroup = new Dictionary<string, List<string>>();

        public static void Initialize(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public static void AddToCache<T>(string key, T value, TimeSpan expiration, string group = null)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });

            if (group != null)
            {
                if (!_cacheKeysByGroup.ContainsKey(group))
                {
                    _cacheKeysByGroup[group] = new List<string>();
                }
                _cacheKeysByGroup[group].Add(key); // Cache anahtarını gruba ekle
            }
        }

        public static void RemoveByGroup(string group)
        {
            if (_cacheKeysByGroup.ContainsKey(group))
            {
                foreach (var key in _cacheKeysByGroup[group])
                {
                    _memoryCache.Remove(key); // Cache'den anahtarı sil
                }
                _cacheKeysByGroup.Remove(group); // Grubu temizle
            }
        }


        public static void ClearAll()
        {
            foreach (var group in _cacheKeysByGroup.Keys)
            {
                RemoveByGroup(group);
            }
        }

        public static bool TryGetValue<T>(string key,out T value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }




    }
}
