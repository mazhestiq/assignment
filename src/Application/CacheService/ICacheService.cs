namespace Assignment.Application.CacheService;
public interface ICacheService
{
    T? Get<T>(string key);
    void Set(string key, object data);
    void Set(string key, object data, int cacheDurationInSeconds);
    void Clear(string key);
}
