#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using pwm.Views.Dialogs;
using System;

namespace pwm.ViewModels.Dialogs
{
    [INotifyPropertyChanged]
    public partial class CreatePasswordContentDialogViewModel: IViewModel
    {
        public Type View => typeof(CreatePasswordContentDialog);

        private const int MIN_PASSWORD_LENGTH = 8;

        public Language.CreatePasswordContentDialogLanguage Strings => Language.Instance.CreatePasswordContentDialog;

        [ObservableProperty]
        private string password1 = string.Empty;
        partial void OnPassword1Changed(string value)
        {
            VerifyPassword();
        }

        [ObservableProperty]
        private string password2 = string.Empty;
        partial void OnPassword2Changed(string value)
        {
            VerifyPassword();
        }

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool canUse = false;

        public string GetPassword()
        {
            return Password1;
        }

        private void VerifyPassword()
        {
            if (Password1.Length < MIN_PASSWORD_LENGTH)
            {
                ErrorMessage = string.Format(Strings.PasswordVerifyLength, MIN_PASSWORD_LENGTH);
                CanUse = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password1))
            {
                ErrorMessage = Strings.PasswordVerifyMessageEmpty;
                CanUse = false;
                return;
            }

            if (Password1 != Password2)
            {
                ErrorMessage = Strings.PasswordVerifyMessageNotEqual;
                CanUse = false;
                return;
            }

            ErrorMessage = string.Empty;
            CanUse = true;
        }
    }
}
