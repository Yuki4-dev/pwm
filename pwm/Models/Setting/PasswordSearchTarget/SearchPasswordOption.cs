#nullable enable

namespace pwm.Models.Setting.SearchPassword
{
    public class SearchPasswordOption
    {
        public bool Enable { get; set; }

        public SearchPasswordTarget PasswordSearchTarget { get; set; }

        public SearchPasswordMatchType PasswordMatchType { get; set; }

        public SearchPasswordOption(bool enable, SearchPasswordTarget passwordSearchTarget, SearchPasswordMatchType passwordMatchType)
        {
            Enable = enable;
            PasswordSearchTarget = passwordSearchTarget;
            PasswordMatchType = passwordMatchType;
        }
    }
}
