#nullable enable

namespace pwm.Models.DialogService
{
    public class ShowMessageDialogParameter
    {
        public string Title { get; }

        public string Message { get; }

        public string PrimaryButtonText { get; }

        public string SecondaryButtonText { get; }

        public string CloseButtonText { get; }

        public ShowMessageDialogParameter(string message) : this(string.Empty, message, "OK") { }

        public ShowMessageDialogParameter(string title, string message) : this(title, message, "OK") { }

        public ShowMessageDialogParameter(string title, string message, string primary, string secondary = "", string close = "")
        {
            Title = title;
            Message = message;
            PrimaryButtonText = primary;
            SecondaryButtonText = secondary;
            CloseButtonText = close;
        }
    }
}
