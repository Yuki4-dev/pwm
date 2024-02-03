#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pwm.Models.Setting;
using pwm.Models.Setting.SearchPassword;
using pwm.Service;
using pwm.ViewModels.Dialogs;
using pwm.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace pwm.ViewModels
{
    [ObservableObject]
    public partial class SettingPageViewModel: IViewModel
    {
        public Type View => typeof(SettingPage);

        public Language.SettingPageLanguage Strings => Language.Instance.SettingPage;

        public string AppName => Language.Instance.AppName;

        public string AppVersion => Language.Instance.AppVersion;

        private readonly IAppSettingProvider appSettingProvider;

        private readonly IDialogService dialogService;

        public ICollection<PwmApplicationTheme> ThemeItems { get; } = new List<PwmApplicationTheme>()
            {
                PwmApplicationTheme.System,
                PwmApplicationTheme.Dark,
                PwmApplicationTheme.Light,
            };

        [ObservableProperty]
        private PwmApplicationTheme selectedTheme;
        partial void OnSelectedThemeChanged(PwmApplicationTheme value)
        {
            appSettingProvider.ApplicationTheme = value;
        }

        [ObservableProperty]
        private string passwordLength;
        partial void OnPasswordLengthChanged(string value)
        {
            appSettingProvider.PasswordLength = int.Parse(value);
        }

        [ObservableProperty]
        private bool isNaturalOrderSort;
        partial void OnIsNaturalOrderSortChanged(bool value)
        {
            appSettingProvider.IsNaturalOrderSort = value;
        }

        [ObservableProperty]
        private bool checkedPasswordSearchTargetName;
        partial void OnCheckedPasswordSearchTargetNameChanged(bool value)
        {
            var old = appSettingProvider.PasswordSearchTargetSetting;
            appSettingProvider.PasswordSearchTargetSetting = new PasswordSearchTargetSetting(
                new SearchPasswordOption(value, old.WebSiteNameOption.PasswordSearchTarget, old.WebSiteNameOption.PasswordMatchType),
                old.WebSiteURIOption,
                old.UserIdOption,
                old.DescriptionOption
            );
        }

        [ObservableProperty]
        private bool checkedPasswordSearchTargetURL;
        partial void OnCheckedPasswordSearchTargetURLChanged(bool value)
        {
            var old = appSettingProvider.PasswordSearchTargetSetting;
            appSettingProvider.PasswordSearchTargetSetting = new PasswordSearchTargetSetting(
                old.WebSiteNameOption,
                new SearchPasswordOption(value, old.WebSiteURIOption.PasswordSearchTarget, old.WebSiteURIOption.PasswordMatchType),
                old.UserIdOption,
                old.DescriptionOption
            );
        }

        [ObservableProperty]
        private bool checkedPasswordSearchTargetUserId;
        partial void OnCheckedPasswordSearchTargetUserIdChanged(bool value)
        {
            var old = appSettingProvider.PasswordSearchTargetSetting;
            appSettingProvider.PasswordSearchTargetSetting = new PasswordSearchTargetSetting(
                old.WebSiteNameOption,
                old.WebSiteURIOption,
                new SearchPasswordOption(value, old.UserIdOption.PasswordSearchTarget, old.UserIdOption.PasswordMatchType),
                old.DescriptionOption
            );
        }

        [ObservableProperty]
        private bool checkedPasswordSearchTargetDescription;
        partial void OnCheckedPasswordSearchTargetDescriptionChanged(bool value)
        {
            var old = appSettingProvider.PasswordSearchTargetSetting;
            appSettingProvider.PasswordSearchTargetSetting = new PasswordSearchTargetSetting(
                old.WebSiteNameOption,
                old.WebSiteURIOption,
                old.UserIdOption,
                new SearchPasswordOption(value, old.DescriptionOption.PasswordSearchTarget, old.DescriptionOption.PasswordMatchType)
            );
        }

        public SettingPageViewModel(IAppSettingProvider appSettingProvider, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.appSettingProvider = appSettingProvider;
            applySetting();
        }

        [RelayCommand]
        private async void GitHubRepository()
        {
            _ = await Windows.System.Launcher.LaunchUriAsync(new Uri(Language.Instance.GitHubRepositoryURL));
        }

        [RelayCommand]
        private async void License()
        {
            await ShowMarkDownContentDialogAsync(@"Assets/Text/License.md");
        }

        [RelayCommand]
        private async void ThirdPartyNotice()
        {
            await ShowMarkDownContentDialogAsync(@"Assets/Text/ThirdPartyNotice.md");
        }

        private void applySetting()
        {
            selectedTheme = appSettingProvider.ApplicationTheme;
            passwordLength = appSettingProvider.PasswordLength.ToString();
            isNaturalOrderSort = appSettingProvider.IsNaturalOrderSort;

            var searchSetting = appSettingProvider.PasswordSearchTargetSetting;
            checkedPasswordSearchTargetName = searchSetting.WebSiteNameOption.Enable;
            checkedPasswordSearchTargetURL = searchSetting.WebSiteURIOption.Enable;
            checkedPasswordSearchTargetUserId = searchSetting.UserIdOption.Enable;
            checkedPasswordSearchTargetDescription = searchSetting.DescriptionOption.Enable;
        }

        private async Task ShowMarkDownContentDialogAsync(string filePath)
        {
            var content = await File.ReadAllTextAsync(filePath);
            var viewModel = new NoticeMarkDownContentDialogViewModel
            {
                Text = content
            };
            _ = await dialogService.ShowContentDialogAsync(viewModel);
        }
    }
}
