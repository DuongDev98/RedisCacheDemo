namespace RedisCacheDemo.Services
{
    public interface ICacheService
    {
        Task<string> Get(string key);
        Task Set(string key, object value, TimeSpan time);
        Task Remove(string pattern);
    }
}
