#nullable enable

using Newtonsoft.Json;
using pwm.Core.Store;
using Windows.Storage;

namespace pwm.Store
{
    public class ApplicationLocalSettingStore : IApplicationSettingStore
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public string? GetStringValue(string key)
        {
            return localSettings.Values.ContainsKey(key) ? localSettings.Values[key].ToString() : null;
        }

        public T? GetOrDefault<T>(string key)
        {
            var jsonStr = GetStringValue(key);
            if (jsonStr == null)
            {
                return default;
            }

            T? result = FromJson<T>(jsonStr);
            return result == null ? default : result;
        }

        public bool SetStringValue(string key, string newValue)
        {
            var oldValue = GetStringValue(key);
            if (Equals(oldValue, newValue))
            {
                return false;
            }

            localSettings.Values[key] = newValue;
            return true;
        }

        public bool SetValueOrDefault<T>(string key, T? newValue)
        {
            return newValue == null ? SetValueOrDefault(key, default(T)) : SetStringValue(key, ToJson(newValue!));
        }

        private string ToJson(object jsonObj)
        {
            try
            {
                return JsonConvert.SerializeObject(jsonObj);
            }
            catch { return string.Empty; }
        }

        private T? FromJson<T>(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch { return default; }
        }
    }
}
