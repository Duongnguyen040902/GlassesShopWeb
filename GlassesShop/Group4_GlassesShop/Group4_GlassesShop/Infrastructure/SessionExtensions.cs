using System.Text.Json;

namespace Group4_GlassesShop.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJon(this ISession session,string key,object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
        }
    }
}
