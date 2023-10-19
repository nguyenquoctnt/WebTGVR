using System.Web.SessionState;

namespace VDMMutiline.Core.Cache
{
    public class BaseCacher : ICache
    {
        public void ClearSystemCache()
        {
            SystemCache.Clear();
        }

        public void ClearUserCache()
        {
            UserCacher.Clear();
        }

        public void DBCleanUserCache()
        {
            UserCacher.DBClean();
        }

        public void FlushUserCache(string username, HttpSessionState session)
        {
            UserCacher.Flush(username, session);
        }

        public object GetSystemCache(string key)
        {
            return SystemCache.Get(key);
        }

        public object GetUserCache(string key)
        {
            return UserCacher.Get(key);
        }

        public void ReloadUserCache()
        {
            UserCacher.ReLoad();
        }

        public void RemoveSystemCache(string key)
        {
            SystemCache.Remove(key);
        }

        public void RemoveUserCache(string key)
        {
            UserCacher.Remove(key);
        }

        public void SetSystemCache(string key, object value)
        {
            SystemCache.Remove(key);
            SystemCache.Insert(key, value);
        }

        public void SetUserCache(string key, object value)
        {
            UserCacher.Set(key, value);
        }
    }
}
