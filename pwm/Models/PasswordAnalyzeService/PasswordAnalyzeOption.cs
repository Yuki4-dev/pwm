#nullable enable

namespace pwm.Models.PasswordAnalyzeService
{
    public class PasswordAnalyzeOption
    {
        public string Keyword { get; }

        public PasswordMatchTarget Target { get; }

        public PasswordMatchType MatchType { get; }

        public PasswordAnalyzeOption(string keyword, PasswordMatchTarget target, PasswordMatchType matchType = PasswordMatchType.PartMatch)
        {
            Keyword = keyword;
            Target = target;
            MatchType = matchType;
        }
    }
}
