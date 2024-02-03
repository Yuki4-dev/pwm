#nullable enable

using pwm.Core.Models;

namespace pwm.Models.PasswordDataProvider
{
    public class PasswordDataContext
    {
        public PasswordInformationSetting PasswordInformationSetting { get; }

        public PasswordInformationEntity[] PasswordInformationEntities { get; }

        public PasswordDataContext(PasswordInformationSetting passwordInformationSetting, PasswordInformationEntity[] passwordInformationEntities)
        {
            PasswordInformationSetting = passwordInformationSetting;
            PasswordInformationEntities = passwordInformationEntities;
        }
    }
}
