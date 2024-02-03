#nullable enable

namespace pwm.Models.PasswordAnalyzeService
{
    public class AnalyzePasswordResult
    {
        public bool Success { get; }

        public string ErrorMessage { get; }

        public AnalyzePasswordResult(bool success, string errorMessage = "")
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
