#nullable enable

using pwm.ViewModels.Dialogs;
using pwm.Views.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views.Dialogs
{
    public sealed partial class CreatePasswordContentDialog : ContentDialog, IViewModelContentDialog
    {
        public static readonly DependencyProperty ViewModelProperty
                                    = DependencyProperty.Register(
                                                        nameof(ViewModel),
                                                        typeof(CreatePasswordContentDialogViewModel),
                                                        typeof(CreatePasswordContentDialog),
                                                        new PropertyMetadata(new CreatePasswordContentDialogViewModel()));

        public CreatePasswordContentDialogViewModel ViewModel
        {
            get => (CreatePasswordContentDialogViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewModelContentDialog.ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ContentDialogHelper Helper { get; }

        public CreatePasswordContentDialog()
        {
            InitializeComponent();
            Helper = new ContentDialogHelper(this);
        }

        private void PasswordBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Helper.FlyoutOpen();
        }

        public void ForceClosed()
        {
            //
        }
    }
}
