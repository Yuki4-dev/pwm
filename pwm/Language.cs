#nullable enable

using System;

namespace pwm
{
    public class Language
    {

        public static readonly Language Instance = new();

        public string AppName => "PWM";

        public string AppVersion
        {
            get
            {
                var versionInfo = Windows.ApplicationModel.Package.Current.Id.Version;
                return "Ver " + string.Format(
                           "{0}.{1}.{2}.{3}",
                           versionInfo.Major,
                           versionInfo.Minor,
                           versionInfo.Build,
                           versionInfo.Revision);

            }
        }

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

            public string Search => "Search password";

            public string Save => "Save";

            public string Export => "Export";

            public string Setting => "Setting";

            public string DeleteSelection => "Delete password selection";

            public string Name => "Name";

            public string URL => "URL";

            public string UserId => "UserId";

            public string VisiblePassword => "Visible password.";

            public string AddPassword => "AddPassword password.";

            public string View => "View";

            public string Edit => "Edit password";

            public string Delete => "Delete password";

            public string EditFailedMessage => "Edit password canceled.";

            public string DeleteMessageTitle => "Delete password";

            public string DeleteMessage => "Delete password?" + Environment.NewLine + "{0}";

            public string DeleteSelectionNothing => "Select password nothing.";

            public string DeleteSelectionMessage => "Delete {0} Password?";

            public string ClosingTitle => "Closing";

            public string ClosingMessage => "Don't Save password.";

            public string ClosingSave => "Save";

            public string ClosingDontSave => "Don't Save";

            public string InvalidURLFormat => "Invalid URL format {0}.";
        }

        public class SettingPageLanguage
        {
            public string Setting => "Setting";

            public string ApplicationTheme => "Application theme";

            public string NeedPasswordLength => "Need password length";

            public string NaturalOrderSort => "Natural order sort";

            public string SearchPasswordTarget => "Password search password Target";

            public string SearchPasswordTarget_Name => "Name";

            public string SearchPasswordTarget_URL => "URL";

            public string SearchPasswordTarget_UserId => "UserId";

            public string SearchPasswordTarget_Description => "Description";

            public string About => "About";

            public string SystemDefault => "System default";

            public string Dark => "Dark";

            public string Light => "Light";

            public string SourceCode => "Source Code";

            public string License => "License";

            public string ThirdPartyNotice => "ThirdParty Notice";
        }

        public class CreatePasswordContentDialogLanguage
        {
            public string Title => "Create password";

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
            public string Title => "Input password";

            public string PrimaryButtonText => "OK";

            public string SecondaryButtonText => "Cancel";
        }

        public class PasswordInformationContentDialogLanguage
        {
            public string TitleEdit => "Edit password";

            public string TitleAdd => "Add password";

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
