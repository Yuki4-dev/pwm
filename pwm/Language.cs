#nullable enable

using System;
using System.Reflection;

namespace pwm
{
    public class Language
    {
        public static readonly Language Instance = new();

        public string AppName => "PWM";

        public string AppVersion => "Ver " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string GitHubRepositoryURL = "https://github.com/Yuki4-dev/pwm";

        public string OK => "OK";

        public string Yes => "Yes";

        public string No => "No";

        public string Cancel => "Cancel";

        public string Error => "Error";

        public PasswordPageLanguage PasswordPage { get; } = new();

        public SettingPageLanguage SettingPage { get; } = new();

        public CreatePasswordContentDialogLanguage CreatePasswordContentDialog { get; } = new();

        public InputPasswordContentDialogLanguage InputPasswordContentDialog { get; } = new();

        public PasswordInformationContentDialogLanguage PasswordInformationContentDialog { get; } = new();

        public InitializeContentDialogLanguage InitializeContentDialog { get; } = new();

        public PasswordAnalyzeServiceLanguage PasswordAnalyzeService { get; } = new();

        private Language() { }

        public class PasswordPageLanguage
        {
            public string Password => "Password";

            public string Passwords => "Passwords";

            public string Search => "SearchPassword";

            public string Save => "Save";

            public string Export => "Export";

            public string Setting => "Setting";

            public string DeleteSelection => "DeletePassword Selection";

            public string Name => "Name";

            public string URL => "URL";

            public string UserId => "UserId";

            public string VisiblePassword => "Visible Password.";

            public string AddPassword => "AddPassword Password.";

            public string View => "View";

            public string Edit => "EditPassword";

            public string Delete => "DeletePassword";

            public string EditFailedMessage => "EditPassword Password Canceled.";

            public string DeleteMessageTitle => "DeletePassword Password";

            public string DeleteMessage => "DeletePassword?" + Environment.NewLine + "{0}";

            public string DeleteSelectionNothing => "Select password nothing.";

            public string DeleteSelectionTitle => "DeletePassword Password";

            public string DeleteSelectionMessage => "DeletePassword {0} Password?";

            public string ClosingTitle => "Closing";

            public string ClosingMessage => "Don't Save Password.";

            public string ClosingSave => "Save";

            public string ClosingDontSave => "Don't Save";

            public string InvalidURLFormat => "Invalid URL format {0}.";
        }

        public class SettingPageLanguage
        {
            public string Setting => "Setting";

            public string ApplicationTheme => "Application Theme";

            public string NeedPasswordLength => "Need Password Length";

            public string NaturalOrderSort => "Natural Order Sort";

            public string SearchPasswordTarget => "Password SearchPassword Target";

            public string SearchPasswordTarget_Name => "Name";

            public string SearchPasswordTarget_URL => "URL";

            public string SearchPasswordTarget_UserId => "UserId";

            public string SearchPasswordTarget_Description => "Description";

            public string About => "About";

            public string SystemDefault => "System Default";

            public string Dark => "Dark";

            public string Light => "Light";

            public string SourceCode => "Source Code";

            public string License => "License";

            public string ThirdPartyNotice => "ThirdParty Notice";
        }

        public class CreatePasswordContentDialogLanguage
        {
            public string Title => "Create Password";

            public string PrimaryButtonText => "OK";

            public string SecondaryButtonText => "Cancel";

            public string HeaderPassword1 => "Create a new password.";

            public string HeaderPassword2 => "Please enter your password again for confirmation.";

            public string PasswordVerifyLength => "Password is not at least {0} Length.";

            public string PasswordVerifyMessageEmpty => "Password is empty.";

            public string PasswordVerifyMessageNotEqual => "Enter password not equal.";
        }

        public class InputPasswordContentDialogLanguage
        {
            public string Title => "Input Password";

            public string PrimaryButtonText => "OK";

            public string SecondaryButtonText => "Cancel";
        }

        public class PasswordInformationContentDialogLanguage
        {
            public string TitleEdit => "EditPassword";

            public string TitleAdd => "AddPassword";

            public string TitleView => "View";

            public string PrimaryButtonText => "OK";

            public string SecondaryButtonText => "Cancel";

            public string Name => "Name";

            public string URL => "URL";

            public string UserId => "UserId";

            public string Password => "Password";

            public string Description => "Description";
        }

        public class InitializeContentDialogLanguage
        {
            public string AppDescription => "Modern Password Manager For Windows.";

            public string CreateNew => "Create New";

            public string OpenOne => "Open One";

            public string GitHub => "GitHub";
        }

        public class PasswordAnalyzeServiceLanguage
        {
            public string NameEmpty => "Name is Empty.";

            public string PasswordEmpty => "Password is Empty.";

            public string PasswordLength => "Password length is {0}, Minimum({1}).";
        }
    }
}
