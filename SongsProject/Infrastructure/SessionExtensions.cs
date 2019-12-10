using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SongsProject.Infrastructure
{
    //All session data must be serialized to enable a distributed cache scenario, even when using the in-memory cache.
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value){
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T>(this ISession session, string key) {
            var sessionData = session.GetString(key);
            return sessionData == null
               ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}