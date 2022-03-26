using CSRedis;
using System;
using System.Threading.Tasks;

namespace DTPDapr.Store.Redis
{
    /// <summary>
    /// IStoreProvider基于Redis的实现
    /// </summary>
    public class StoreProviderRedisImpl : IStoreProvider
    {
        static Lazy<CSRedisClient> csRedisClient = new Lazy<CSRedisClient>(new CSRedisClient(ConfigurationManager.GetConfig().StoreConnectionString));
        /// <summary>
        /// 根据Key移除数据
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveKey(string key)
        {
            return await csRedisClient.Value.DelAsync(key) > 0;
        }
        /// <summary>
        /// 写入data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expressTime"></param>
        /// <returns></returns>
        public async Task<bool> SetDataByKey(string key, DTPDaprData data, DateTime expireTime)
        {
            return await csRedisClient.Value.SetAsync(key, data, expireTime - DateTime.Now);
        }
        /// <summary>
        /// key幂等检测
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<DTPDaprData> GetKey(string key)
        {
            return await csRedisClient.Value.GetAsync<DTPDaprData>(key);
        }
    }
}
