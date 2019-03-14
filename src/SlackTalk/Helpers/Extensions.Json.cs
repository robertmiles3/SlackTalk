using Newtonsoft.Json;

namespace SlackTalk
{
    internal static class JsonExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, JsonSerializerSettings);
        }
        
        internal static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
        }
    }
}