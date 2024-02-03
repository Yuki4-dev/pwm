#nullable enable

using pwm.ViewModels.Dialogs;
using pwm.Views.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views.Dialogs
{
    public sealed partial class InputPasswordContentDialog : ContentDialog, IViewModelContentDialog
    {
        public static readonly DependencyProperty ViewModelProperty
                                    = DependencyProperty.Register(
                                                        nameof(ViewModel),
                                                        typeof(InputPasswordContentDialogViewModel),
                                                        typeof(InputPasswordContentDialog),
                                                        new PropertyMetadata(null));

        public InputPasswordContentDialogViewModel ViewModel
        {
            get => (InputPasswordContentDialogViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewModelContentDialog.ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ContentDialogHelper Helper { get; }

        public InputPasswordContentDialog()
        {
            InitializeComponent();
            Helper = new ContentDialogHelper(this)
            {
                IsBackgroundPointerDownClosing = false
            };
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var options = new FindNextElementOptions()
                {
                    SearchRoot = this,
                    XYFocusNavigationStrategyOverride = XYFocusNavigationStrategyOverride.Projection
                };

                var candidate = FocusManager.FindNextElement(FocusNavigationDirection.Down, options);
                if (candidate is not null and Control)
                {
                    _ = ((candidate as Control)?.Focus(FocusState.Keyboard));
                }
            }
        }

        public void ForceClosed()
        {
            //
        }
    }
}
