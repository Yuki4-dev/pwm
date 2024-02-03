#nullable enable

using pwm.Models.Setting.SearchPassword;

namespace pwm.Models.Setting
{
    public class PasswordSearchTargetSetting
    {
        public static readonly PasswordSearchTargetSetting Default = new();

        public SearchPasswordOption WebSiteNameOption { get; }

        public SearchPasswordOption WebSiteURIOption { get; }

        public SearchPasswordOption UserIdOption { get; }

        public SearchPasswordOption DescriptionOption { get; }

        public PasswordSearchTargetSetting() : this(
            new SearchPasswordOption(true, SearchPasswordTarget.WebSiteName, SearchPasswordMatchType.PartMatch),
            new SearchPasswordOption(true, SearchPasswordTarget.WebSiteURI, SearchPasswordMatchType.PartMatch),
            new SearchPasswordOption(true, SearchPasswordTarget.UserId, SearchPasswordMatchType.PartMatch),
            new SearchPasswordOption(true, SearchPasswordTarget.Description, SearchPasswordMatchType.PartMatch)
            )
        { }

        public PasswordSearchTargetSetting(SearchPasswordOption webSiteNameOption, SearchPasswordOption webSiteURIOption, SearchPasswordOption userIdNameOption, SearchPasswordOption descriptionOption)
        {
            WebSiteNameOption = webSiteNameOption;
            WebSiteURIOption = webSiteURIOption;
            UserIdOption = userIdNameOption;
            DescriptionOption = descriptionOption;
        }
    }
}
