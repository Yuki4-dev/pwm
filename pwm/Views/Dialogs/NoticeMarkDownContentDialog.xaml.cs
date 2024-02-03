#nullable enable

using pwm.ViewModels.Dialogs;
using pwm.Views.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views.Dialogs
{
    public sealed partial class NoticeMarkDownContentDialog : ContentDialog, IViewModelContentDialog
    {
        public static readonly DependencyProperty ViewModelProperty
                            = DependencyProperty.Register(
                                                nameof(ViewModel),
                                                typeof(NoticeMarkDownContentDialogViewModel),
                                                typeof(NoticeMarkDownContentDialog),
                                                new PropertyMetadata(null));

        public NoticeMarkDownContentDialogViewModel ViewModel
        {
            get => (NoticeMarkDownContentDialogViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewModelContentDialog.ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ContentDialogHelper Helper { get; }

        public NoticeMarkDownContentDialog()
        {
            InitializeComponent();
            PrimaryButtonText = pwm.Language.Instance.OK;

            Helper = new ContentDialogHelper(this)
            {
                IsBackgroundPointerDownClosing = false
            };
        }

        public void ForceClosed()
        {
        }
    }
}
