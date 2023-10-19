using System;
using System.Collections;

namespace VDMMutiline.Core.Cache
{
    internal class SystemCache
    {
        private static ISystemCache _provider = new CacheRuntime();

        public static void Clear()
        {
            _provider.Clear();
        }

        public static bool Contains(string key)
        {
            return _provider.Contains(key);
        }

        public static object Get(object key)
        {
            return _provider.Get(key);
        }

        public static T Get<T>(object key)
        {
            return _provider.Get<T>(key);
        }

        public static T Get<T>(string key, int timeInSeconds, Func<T> fetcher)
        {
            return Get<T>(key, true, timeInSeconds, false, fetcher);
        }

        public static T Get<T>(string key, TimeSpan timeInSeconds, Func<T> fetcher)
        {
            return Get<T>(key, true, timeInSeconds.Seconds, false, fetcher);
        }

        public static T Get<T>(string key, bool useCache, TimeSpan timeInSeconds, Func<T> fetcher)
        {
            return Get<T>(key, useCache, timeInSeconds.Seconds, false, fetcher);
        }

        public static T Get<T>(string key, bool useCache, int timeInSeconds, bool slidingExpiration, Func<T> fetcher)
        {
            if (!useCache)
            {
                return fetcher();
            }
            return _provider.GetOrInsert<T>(key, timeInSeconds, slidingExpiration, fetcher);
        }

        private static void Init(ISystemCache cacheProvider)
        {
            _provider = cacheProvider;
        }

        public static void Insert(object key, object value)
        {
            _provider.Insert(key, value);
        }

        public static void Insert(object key, object value, int timeToLive, bool slidingExpiration)
        {
            _provider.Insert(key, value, timeToLive, slidingExpiration);
        }

        public static void Insert(object key, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority)
        {
            _provider.Insert(key, value, timeToLive, slidingExpiration, priority);
        }

        public static void Remove(object key)
        {
            _provider.Remove(key);
        }

        public static void RemoveAll(ICollection keys)
        {
            _provider.RemoveAll(keys);
        }

        public static int Count
        {
            get
            {
                return _provider.Count;
            }
        }

        public static ICollection Keys
        {
            get
            {
                return _provider.Keys;
            }
        }

        private static ISystemCache Provider
        {
            get
            {
                return _provider;
            }
        }
    }
}
