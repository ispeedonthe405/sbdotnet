using System.Text.Json;

namespace sbdotnet
{
    public static class Json
    {
        public static string ToJson<T>(this List<T> collection)
        {
            try
            {
                return JsonSerializer.Serialize(collection);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return string.Empty;
        }

        public static void FromJson<T>(this List<T> collection, string json)
        {
            try
            {
                collection = JsonSerializer.Deserialize<List<T>?>(json) ?? [];
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
