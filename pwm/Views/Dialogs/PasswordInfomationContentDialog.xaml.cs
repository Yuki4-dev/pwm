#nullable enable

using pwm.ViewModels.Dialogs;
using pwm.Views.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views.Dialogs
{
    public sealed partial class PasswordInformationContentDialog : ContentDialog, IViewModelContentDialog
    {
        public static readonly DependencyProperty ViewModelProperty
                                    = DependencyProperty.Register(
                                                        nameof(ViewModel),
                                                        typeof(PasswordInformationContentDialogViewModel),
                                                        typeof(PasswordInformationContentDialog),
                                                        new PropertyMetadata(null, ChangedViewModel));

        private static void ChangedViewModel(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is PasswordInformationContentDialogViewModel viewModel)
            {
                var dialog = (PasswordInformationContentDialog)d;
                dialog.DataContext = viewModel;
                dialog.PasswordTextBox.Visibility = viewModel.HiddenPassword ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public PasswordInformationContentDialogViewModel ViewModel
        {
            get => (PasswordInformationContentDialogViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewModelContentDialog.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PasswordInformationContentDialogViewModel)value;
        }

        public ContentDialogHelper Helper { get; }

        public PasswordInformationContentDialog()
        {
            InitializeComponent();
            Helper = new ContentDialogHelper(this);
        }

        private void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            Helper.FlyoutOpen();
        }

        public void ForceClosed()
        {
            //
        }
    }
}
