#nullable enable


namespace pwm.Models.PasswordAnalyzeService
{
    public class MatchPasswordResult
    {
        public bool IsMatch { get; }

        public PasswordAnalyzeInformation[] Matches { get; }

        public MatchPasswordResult(bool isMatch, PasswordAnalyzeInformation[] information)
        {
            IsMatch = isMatch;
            Matches = information;
        }
    }
}
