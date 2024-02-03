#nullable enable

namespace pwm.Core.Store
{
    public interface IApplicationSettingStore
    {
        public string? GetStringValue(string key);

        public T? GetOrDefault<T>(string key);

        public bool SetStringValue(string key, string newValue);

        public bool SetValueOrDefault<T>(string key, T? newValue);
    }
}
