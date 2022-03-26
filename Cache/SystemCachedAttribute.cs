namespace MultilevelCache
{

    /// <summary>
    /// 缓存注解
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SystemCachedAttribute : Attribute
    {
        public int ExpireSecond { get; set; }
        public int TimeOutMillisecond { get; set; }
        public SystemCachedType CachedType { get; set; }
        public bool SyncVisit { get; set; }
        /// <summary>
        /// 缓存注解
        /// </summary>
        /// <param name="expireSecond">设置缓存时间,默认60秒,0为不缓存，单位秒</param>
        /// <param name="timeOutMillisecond">设置查询缓存超时时间,默认5000毫秒，0为不设置超时,单位毫秒</param>
        /// <param name="cachedType">设置缓存类型，默认按方法缓存，Method按方法名缓存，MethodAndParams按方法和参数缓存</param>
        /// <param name="singleVisit">当缓存失效时访问函数是否采用线程同步访问模式，默认采用</param>
        public SystemCachedAttribute(int expireSecond = 60, int timeOutMillisecond = 5000, SystemCachedType cachedType = SystemCachedType.Method, bool syncVisit = true)
        {
            this.ExpireSecond = expireSecond;
            this.CachedType = cachedType;
            this.TimeOutMillisecond = timeOutMillisecond;
            this.SyncVisit = syncVisit;
        }
    }

    public enum SystemCachedType
    {
        Method = 0,
        MethodAndParams = 1
    }
}