#nullable enable

using pwm.Models.DialogService;

namespace pwm.Models.PasswordPage
{
    public static class PasswordPageShowMessages
    {
        public static readonly ShowMessageDialogParameter EditFailedMessage = new(Language.Instance.PasswordPage.EditFailedMessage);

        public static readonly ShowMessageDialogParameter ClosingMessage = new(Language.Instance.PasswordPage.ClosingTitle,
            Language.Instance.PasswordPage.ClosingMessage, Language.Instance.PasswordPage.ClosingSave, Language.Instance.PasswordPage.ClosingDontSave, Language.Instance.Cancel);

        public static readonly ShowMessageDialogParameter DeleteSelectionNothingMessage = new(Language.Instance.PasswordPage.DeleteSelectionNothing);

        public static ShowMessageDialogParameter GetDeleteMessage(PasswordInformation deleteInformation)
        {
            var message = string.Format(Language.Instance.PasswordPage.DeleteMessage, deleteInformation.ToString());
            return new ShowMessageDialogParameter(Language.Instance.PasswordPage.DeleteMessageTitle, message, Language.Instance.Yes, Language.Instance.No);
        }

        public static ShowMessageDialogParameter GetDeleteSelection(int deleteCount)
        {
            var message = string.Format(Language.Instance.PasswordPage.DeleteSelectionMessage, deleteCount);
            return new ShowMessageDialogParameter(Language.Instance.PasswordPage.DeleteMessageTitle, message, Language.Instance.Yes, Language.Instance.No);
        }

        public static ShowMessageDialogParameter GetInvalidURLFormat(string uri)
        {
            var message = string.Format(Language.Instance.PasswordPage.InvalidURLFormat, uri);
            return new ShowMessageDialogParameter(message);
        }
    }
}
