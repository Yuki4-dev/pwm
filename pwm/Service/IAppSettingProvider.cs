#nullable enable

using pwm.Models.Setting;
using System;

namespace pwm.Service
{
    public interface IAppSettingProvider
    {
        public event EventHandler? SettingChanged;

        public PwmApplicationTheme ApplicationTheme { get; set; }

        public int PasswordLength { get; set; }

        public bool IsNaturalOrderSort { get; set; }

        public PasswordSearchTargetSetting PasswordSearchTargetSetting { get; set; }
    }
}
