using System;
using System.Collections;

namespace VDMMutiline.Core.Cache
{
    internal interface ISystemCache
    {
        void Clear();
        bool Contains(string key);
        object Get(object key);
        T Get<T>(object key);
        T GetOrInsert<T>(object key, int timeToLiveInSeconds, bool slidingExpiration, Func<T> fetcher);
        void Insert(object key, object value);
        void Insert(object key, object value, int timeToLive, bool slidingExpiration);
        void Insert(object key, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority);
        void Remove(object key);
        void RemoveAll(ICollection keys);

        int Count { get; }

        ICollection Keys { get; }
    }
}
