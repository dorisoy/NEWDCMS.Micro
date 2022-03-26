namespace MultilevelCache
{
    public interface IL1CacheServiceFactory
    {
        public T Get<T>(string key);
        public bool Set<T>(string key, T value, int expireTimeSecond = 0);
    }
    public interface IL2CacheServiceFactory
    {
        public Task<T> GetAsync<T>(string key);
        public Task<bool> SetAsync<T>(string key, T value, int expireTimeSecond = 0);
    }
}