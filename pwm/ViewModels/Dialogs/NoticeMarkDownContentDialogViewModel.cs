using CommunityToolkit.Mvvm.ComponentModel;
using pwm.Views.Dialogs;
using System;

namespace pwm.ViewModels.Dialogs
{
    [INotifyPropertyChanged]
    public partial class NoticeMarkDownContentDialogViewModel: IViewModel
    {
        public Type View => typeof(NoticeMarkDownContentDialog);

        [ObservableProperty]
        private string text = string.Empty;
    }
}
