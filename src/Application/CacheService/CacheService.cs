using System.Collections.Concurrent;

namespace Assignment.Application.CacheService;

public class CacheService : ICacheService
{
    private readonly CacheConfig _cacheConfig;
    private readonly ConcurrentDictionary<string, CacheItem> _cacheStorage = new();

    public CacheService()
    {
        _cacheConfig = new CacheConfig { DefaultCacheDurationInSeconds = 100 };
    }

    public T? Get<T>(string key)
    {
        if(!_cacheStorage.ContainsKey(key))
            return default;

        var cacheItem = _cacheStorage[key];

        if (DateTime.UtcNow <= cacheItem.ExpireAt)
        {
            return (T)cacheItem.Item;
        }

        Clear(key);
        return default;
    }

    public void Set(string key, object data)
    {
        var cacheItem = new CacheItem(data, _cacheConfig.DefaultCacheDurationInSeconds);

        _cacheStorage.TryAdd(key, cacheItem);
    }

    public void Set(string key, object data, int cacheDurationInSeconds)
    {
        var cacheItem = new CacheItem(data, cacheDurationInSeconds);

        _cacheStorage.TryAdd(key, cacheItem);
    }

    public void Clear(string key)
    {
        _cacheStorage.TryRemove(key, out CacheItem? _);
    }
}
