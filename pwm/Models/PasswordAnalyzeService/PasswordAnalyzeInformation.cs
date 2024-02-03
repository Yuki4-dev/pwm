#nullable enable

namespace pwm.Models.PasswordAnalyzeService
{
    public class PasswordAnalyzeInformation
    {
        public string Keyword { get; }

        public string Value { get; }

        public int Index { get; }

        public int Length { get; }

        public PasswordMatchTarget Target { get; }

        public PasswordMatchType MatchType { get; }

        public PasswordAnalyzeInformation(string keyword, string value, int index, int length, PasswordMatchTarget target, PasswordMatchType matchType)
        {
            Keyword = keyword;
            Value = value;
            Index = index;
            Length = length;
            Target = target;
            MatchType = matchType;
        }
    }

}
