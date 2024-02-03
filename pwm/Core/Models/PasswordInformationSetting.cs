#nullable enable

using Windows.Storage;

namespace pwm.Core.Models
{
    public class PasswordInformationSetting
    {
        public IStorageFile OpenFile { get; }

        public string Password { get; }

        public PasswordInformationSetting(IStorageFile file, string password)
        {
            OpenFile = file;
            Password = password;
        }
    }
}
