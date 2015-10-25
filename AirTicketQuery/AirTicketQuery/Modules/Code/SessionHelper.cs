using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AirTicketQuery.Modules.Code
{
    public class SessionHelper
    {
        public static void SetValutToSession<T>(string key, T value)
        {
            SetValutToSession<T>(HttpContext.Current, key, value);
        }

        private static void SetValutToSession<T>(HttpContext context, string key, T value)
        {
            HttpSessionState session = context.Session;
            if (session != null)
            {
                if (!key.StartsWith("TEMP_"))
                    session.Timeout = 120;
                session[key] = value;
            }
        }

        public static T GetValueFromSession<T>(string key)
        {
            return GetValueFromSession<T>(HttpContext.Current, key, default(T));
        }

        public static T GetValueFromSession<T>(string key, T defaulValue)
        {
            return GetValueFromSession<T>(HttpContext.Current, key, defaulValue);
        }

        private static T GetValueFromSession<T>(HttpContext context, string key)
        {
            return GetValueFromSession<T>(HttpContext.Current, key, default(T));
        }

        private static T GetValueFromSession<T>(HttpContext context, string key, T defaulValue)
        {
            HttpSessionState session = context.Session;
            if (session != null && session[key] is T)
            {
                if (!key.StartsWith("TEMP_"))
                    session.Timeout = 120;
                return (T)session[key];
            }
            else
            {
                return defaulValue;
            }
        }

        public static bool CheckValueOfSession(string key)
        {
            return CheckValueOfSession(HttpContext.Current, key);
        }

        private static bool CheckValueOfSession(HttpContext context, string key)
        {
            HttpSessionState session = context.Session;
            if (session != null && session[key] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Clear(string key)
        {
            Clear(HttpContext.Current, key);
        }

        private static void Clear(HttpContext context, string key)
        {
            HttpSessionState session = context.Session;
            if (session != null && session[key] != null)
            {
                session.Timeout = 120;
                session.Remove(key);
            }
        }
    }
}
