using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;

namespace VDMMutiline.Core.Cache
{
    internal class CacheRuntime : ISystemCache
    {
        private static MemoryCache _cache;
        private CacheSettings _settings;

        public CacheRuntime()
        {
            this._settings = new CacheSettings();
            int num = 15;
            try
            {
                if (ConfigurationManager.AppSettings["CacheTimeoutInMinutes"] != null)
                {
                    num = Convert.ToInt32(ConfigurationManager.AppSettings["CacheTimeoutInMinutes"].ToString());
                }
            }
            catch (Exception)
            {
                num = 15;
            }
            this._settings.DefaultTimeToLive = 0xea60 * num;
            this._settings.DefaultSlidingExpirationEnabled = true;
            this._settings.DefaultCachePriority = CacheItemPriority.NotRemovable;
            this.Init(this._settings);
        }

        public CacheRuntime(CacheSettings settings)
        {
            this._settings = new CacheSettings();
            this.Init(settings);
        }

        private string BuildKey(object key)
        {
            if (this._settings.UsePrefix)
            {
                return (this._settings.PrefixForCacheKeys + "." + key.ToString());
            }
            return key.ToString();
        }

        public void Clear()
        {
            ICollection keys = this.Keys;
            foreach (string str in keys)
            {
                string key = this.BuildKey(str);
                _cache.Remove(key, null);
            }
        }

        public bool Contains(string key)
        {
            string str = this.BuildKey(key);
            if (_cache.Get(str, null) == null)
            {
                return false;
            }
            return true;
        }

        public object Get(object key)
        {
            if (key == null)
            {
                return null;
            }
            string str = this.BuildKey(key);
            return _cache.Get(str, null);
        }

        public T Get<T>(object key)
        {
            string str = this.BuildKey(key);
            object obj2 = _cache.Get(str, null);
            if (obj2 == null)
            {
                return default(T);
            }
            return (T)obj2;
        }

        public T GetOrInsert<T>(object key, int timeToLiveInSeconds, bool slidingExpiration, Func<T> fetcher)
        {
            object obj2 = this.Get(key);
            if (obj2 == null)
            {
                T local = fetcher();
                this.Insert(key, local, timeToLiveInSeconds, slidingExpiration);
                return local;
            }
            return (T)obj2;
        }

        private void Init(CacheSettings settings)
        {
            if (_cache == null)
            {
                _cache = new MemoryCache("SystemCache", null);
            }
            this._settings = settings;
        }

        public void Insert(object key, object value)
        {
            this.Insert(key, value, this._settings.DefaultTimeToLive, this._settings.DefaultSlidingExpirationEnabled, this._settings.DefaultCachePriority);
        }

        public void Insert(object key, object value, int timeToLive, bool slidingExpiration)
        {
            this.Insert(key, value, timeToLive, slidingExpiration, _settings.DefaultCachePriority);
        }

        public void Insert(object keyName, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority)
        {
            TimeSpan span = TimeSpan.FromMilliseconds((double)timeToLive);
            priority = CacheItemPriority.NotRemovable;
            string key = this.BuildKey(keyName);
            if (TimeSpan.Zero < span)
            {
                CacheItemPolicy policy;
                if (slidingExpiration)
                {
                    policy = new CacheItemPolicy();
                    if (priority == CacheItemPriority.NotRemovable)
                    {
                        policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
                    }
                    else
                    {
                        policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
                    }
                    policy.SlidingExpiration = span;
                    _cache.Add(new CacheItem(key, value), policy);
                }
                else
                {
                    policy = new CacheItemPolicy();
                    if (priority == CacheItemPriority.NotRemovable)
                    {
                        policy.Priority = System.Runtime.Caching.CacheItemPriority.NotRemovable;
                    }
                    else
                    {
                        policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;
                    }
                    DateTime time = DateTime.Now.AddSeconds((double)timeToLive);
                    policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMilliseconds((double)timeToLive));
                    _cache.Add(new CacheItem(key, value), policy);
                }
            }
            else
            {
                this.Insert(key, value);
            }
        }

        public void Remove(object key)
        {
            string str = this.BuildKey(key);
            _cache.Remove(str, null);
        }

        public void RemoveAll(ICollection keys)
        {
            foreach (object obj2 in keys)
            {
                string key = this.BuildKey((string)obj2);
                _cache.Remove(key, null);
            }
        }

        public int Count
        {
            get
            {
                if (!this._settings.UsePrefix)
                {
                    return (int)_cache.GetCount(null);
                }
                int num = 0;
                foreach (KeyValuePair<string, object> pair in (IEnumerable<KeyValuePair<string, object>>)_cache)
                {
                    if (pair.Key.StartsWith(this._settings.PrefixForCacheKeys))
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public ICollection Keys
        {
            get
            {
                IList<string> list = new List<string>();
                foreach (KeyValuePair<string, object> pair in (IEnumerable<KeyValuePair<string, object>>)_cache)
                {
                    string key = pair.Key;
                    if (!this._settings.UsePrefix)
                    {
                        list.Add(key);
                    }
                    else if (key.StartsWith(this._settings.PrefixForCacheKeys))
                    {
                        list.Add(key.Substring(this._settings.PrefixForCacheKeys.Length + 1));
                    }
                }
                return (list as ICollection);
            }
        }

        public CacheSettings Settings
        {
            get
            {
                return this._settings;
            }
        }
    }
}
