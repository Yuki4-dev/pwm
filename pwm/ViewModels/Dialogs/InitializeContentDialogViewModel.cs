#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using pwm.Models.InitializeContentDialog;
using pwm.Views.Dialogs;
using System;

namespace pwm.ViewModels.Dialogs
{
    [INotifyPropertyChanged]
    public partial class InitializeContentDialogViewModel : IViewModel
    {
        public Type View => typeof(InitializeContentDialog);

        public Language.InitializeContentDialogLanguage Strings => Language.Instance.InitializeContentDialog;

        public string AppName => Language.Instance.AppName;

        public InitializeContentDialogResult Result { get; private set; } = InitializeContentDialogResult.None;

        [RelayCommand]
        private void CreateNew()
        {
            Result = InitializeContentDialogResult.CreateNew;
        }

        [RelayCommand]
        private void OpenOne()
        {
            Result = InitializeContentDialogResult.OpenOne;
        }

        [RelayCommand]
        private async void GitHub()
        {
            _ = await Windows.System.Launcher.LaunchUriAsync(new Uri(Language.Instance.GitHubRepositoryURL));
        }
    }
}
