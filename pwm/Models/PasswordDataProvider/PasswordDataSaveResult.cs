#nullable enable

namespace pwm.Models.PasswordDataProvider
{
    public class PasswordDataSaveResult
    {
        public static readonly PasswordDataSaveResult Canceled = new();

        public bool IsDone { get; } = false;

        public PasswordDataContext? PasswordDataContext { get; }

        private PasswordDataSaveResult() { }

        public PasswordDataSaveResult(bool isDone, PasswordDataContext passwordDataContext)
        {
            IsDone = isDone;
            PasswordDataContext = passwordDataContext;
        }
    }
}
