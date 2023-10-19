using System.Web.SessionState;

namespace VDMMutiline.Core.Cache
{
    public interface ICache
    {
        void ClearSystemCache();
        void ClearUserCache();
        void DBCleanUserCache();
        void FlushUserCache(string username, HttpSessionState session);
        object GetSystemCache(string key);
        object GetUserCache(string key);
        void ReloadUserCache();
        void RemoveSystemCache(string key);
        void RemoveUserCache(string key);
        void SetSystemCache(string key, object value);
        void SetUserCache(string key, object value);
    }
}
