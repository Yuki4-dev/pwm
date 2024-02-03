#nullable enable

using pwm.Core.Store;
using pwm.Models.Setting;
using System;

namespace pwm.Service.Setting
{
    public class AppSettingProvider : IAppSettingProvider
    {
        private readonly IApplicationSettingStore applicationSettingStore;

        public AppSettingProvider(IApplicationSettingStore applicationSettingStore)
        {
            this.applicationSettingStore = applicationSettingStore;
        }

        public PwmApplicationTheme ApplicationTheme
        {
            get
            {
                var enumStr = applicationSettingStore.GetStringValue(nameof(ApplicationTheme)) ?? PwmApplicationTheme.System.ToString();
                return (PwmApplicationTheme)Enum.Parse(typeof(PwmApplicationTheme), enumStr);
            }
            set
            {
                if (applicationSettingStore.SetStringValue(nameof(ApplicationTheme), value.ToString()))
                {
                    OnSettingChanged();
                }
            }
        }

        public int PasswordLength
        {
            get
            {
                var lengthStr = applicationSettingStore.GetStringValue(nameof(PasswordLength)) ?? "8";
                return int.Parse(lengthStr);

            }
            set
            {
                if (applicationSettingStore.SetStringValue(nameof(PasswordLength), value.ToString()))
                {
                    OnSettingChanged();
                }
            }
        }

        public bool IsNaturalOrderSort
        {
            get
            {
                var boolStr = applicationSettingStore.GetStringValue(nameof(IsNaturalOrderSort)) ?? true.ToString();
                return bool.Parse(boolStr);
            }
            set
            {
                if (applicationSettingStore.SetStringValue(nameof(IsNaturalOrderSort), value.ToString()))
                {
                    OnSettingChanged();
                }
            }
        }

        public PasswordSearchTargetSetting PasswordSearchTargetSetting
        {
            get
            {
                var setting = applicationSettingStore.GetOrDefault<PasswordSearchTargetSetting>(nameof(PasswordSearchTargetSetting));
                return setting == null ? PasswordSearchTargetSetting.Default : setting;
            }
            set
            {
                if(applicationSettingStore.SetValueOrDefault(nameof(PasswordSearchTargetSetting), value))
                {
                    OnSettingChanged();
                }
            }
        }

        public event EventHandler? SettingChanged;
        private void OnSettingChanged()
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
