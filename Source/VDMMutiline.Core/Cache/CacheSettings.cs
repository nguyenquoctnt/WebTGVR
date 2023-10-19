namespace VDMMutiline.Core.Cache
{
    internal class CacheSettings
    {
        public CacheItemPriority DefaultCachePriority = CacheItemPriority.Normal;
        public bool DefaultSlidingExpirationEnabled = false;
        public int DefaultTimeToLive = 0x15180;
        public string PrefixForCacheKeys = "cmnlib";
        public bool UsePrefix = true;
    }
}
