using System.Collections;
using System.Web;
using System.Web.SessionState;

namespace VDMMutiline.Core.Cache
{
    internal class UserCacher
    {
        public static void Clear()
        {
            Hashtable cache = new Hashtable();
            Update(cache, HttpContext.Current.Session);
        }

        public static void DBClean()
        {
        }

        public static void Flush(string username)
        {
        }

        public static void Flush(string username, HttpSessionState session)
        {
        }

        public static object Get(string key)
        {
            return Get(key, HttpContext.Current.Session);
        }

        public static object Get(string key, HttpSessionState session)
        {
            if ((session != null) && (session["cache"] != null))
            {
                Hashtable hashtable = (Hashtable)session["cache"];
                if (hashtable.Contains(key))
                {
                    return hashtable[key];
                }
            }
            return null;
        }

        public static void ReLoad()
        {
        }

        public static void Remove(string key)
        {
            if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["cache"] != null))
            {
                Hashtable cache = (Hashtable)HttpContext.Current.Session["cache"];
                if ((cache != null) && cache.Contains(key))
                {
                    cache.Remove(key);
                }
                Update(cache, HttpContext.Current.Session);
            }
        }

        public static void Set(string key, object value)
        {
            if (HttpContext.Current.Session != null)
            {
                Hashtable hashtable;
                if (HttpContext.Current.Session["cache"] == null)
                {
                    hashtable = new Hashtable();
                }
                else
                {
                    hashtable = (Hashtable)HttpContext.Current.Session["cache"];
                }
                if (hashtable.Contains(key))
                {
                    hashtable.Remove(key);
                }
                hashtable.Add(key, value);
                Update(hashtable, HttpContext.Current.Session);
            }
        }

        public static void Set(string key, object value, HttpSessionState session)
        {
            if (session != null)
            {
                Hashtable hashtable;
                if (session["cache"] == null)
                {
                    hashtable = new Hashtable();
                }
                else
                {
                    hashtable = (Hashtable)session["cache"];
                }
                if (hashtable.Contains(key))
                {
                    hashtable.Remove(key);
                }
                hashtable.Add(key, value);
                Update(hashtable, session);
            }
        }

        private static void Update(Hashtable cache, HttpSessionState session)
        {
            if (session != null)
            {
                session["cache"] = cache;
            }
        }
    }
}
