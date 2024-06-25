namespace Assignment.Application.CacheService;

public class CacheItem
{
    public object Item { get; set; }

    public DateTime ExpireAt { get; set; }

    public CacheItem(object item, int durationInSeconds)
    {
        Item = item;
        ExpireAt = DateTime.UtcNow.AddSeconds(durationInSeconds);
    }
}
