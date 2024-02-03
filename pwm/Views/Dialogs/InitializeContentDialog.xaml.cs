#nullable enable

using pwm.ViewModels.Dialogs;
using pwm.Views.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views.Dialogs
{
    public sealed partial class InitializeContentDialog : ContentDialog, IViewModelContentDialog
    {
        public static readonly DependencyProperty ViewModelProperty
                            = DependencyProperty.Register(
                                                nameof(ViewModel),
                                                typeof(InitializeContentDialogViewModel),
                                                typeof(InitializeContentDialog),
                                                new PropertyMetadata(null));

        public InitializeContentDialogViewModel ViewModel
        {
            get => (InitializeContentDialogViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewModelContentDialog.ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ContentDialogHelper Helper { get; }

        public InitializeContentDialog()
        {
            InitializeComponent();
            Helper = new ContentDialogHelper(this)
            {
                IsBackgroundPointerDownClosing = false
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        public void ForceClosed()
        {
            //
        }
    }
}
