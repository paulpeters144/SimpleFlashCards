using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SimpleFlashCards.Helpers
{
    public static class Session
    {
        public static void Set(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static void SetUserId(this ISession session, string value)
        {
            session.SetString("userId", value);
        }

        public static string Get(this ISession session, string key)
        {
            if (hasKey(session, key))
                return session.GetString(key);

            return null;
        }

        public static string GetId(this ISession session)
        {
            if (hasKey(session, "userId"))
                return session.GetString("userId");

            return "";
        }

        internal static bool isLoggedOn(this ISession session)
        {
            return session.hasKey("userId");
        }

        private static bool hasKey(this ISession session, string key)
        {
            var keys = session.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
                if (String.Equals(keys[i].ToString(), key))
                    return true;
            return false;
        }
    }
}
