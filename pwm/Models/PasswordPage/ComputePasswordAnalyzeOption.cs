#nullable enable

using pwm.Models.PasswordAnalyzeService;
using pwm.Models.Setting;
using System.Collections.Generic;

namespace pwm.Models.PasswordPage
{
    public class ComputePasswordAnalyzeOption
    {
        public PasswordAnalyzeOption[] Options { get; }

        public ComputePasswordAnalyzeOption(PasswordSearchTargetSetting searchTargetSetting, string searchedKeyWord)
        {
            var matches = new List<PasswordAnalyzeOption>();
            if (searchTargetSetting.WebSiteNameOption.Enable)
            {
                matches.Add(new PasswordAnalyzeOption(searchedKeyWord, PasswordMatchTarget.WebSiteName));
            }

            if (searchTargetSetting.WebSiteURIOption.Enable)
            {
                matches.Add(new PasswordAnalyzeOption(searchedKeyWord, PasswordMatchTarget.WebSiteURI));
            }

            if (searchTargetSetting.UserIdOption.Enable)
            {
                matches.Add(new PasswordAnalyzeOption(searchedKeyWord, PasswordMatchTarget.UserId));
            }

            if (searchTargetSetting.DescriptionOption.Enable)
            {
                matches.Add(new PasswordAnalyzeOption(searchedKeyWord, PasswordMatchTarget.Description));
            }

            Options = matches.ToArray();
        }
    }
}
