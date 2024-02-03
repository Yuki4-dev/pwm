#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using pwm.Views.Dialogs;
using System;

namespace pwm.ViewModels.Dialogs
{
    [INotifyPropertyChanged]
    public partial class InputPasswordContentDialogViewModel : IViewModel
    {
        public Type View => typeof(InputPasswordContentDialog);

        public Language.InputPasswordContentDialogLanguage Strings => Language.Instance.InputPasswordContentDialog;

        [ObservableProperty]
        private string password = string.Empty;
    }
}
