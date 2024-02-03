#nullable enable

using pwm.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace pwm.Views.UI
{
    public partial class ContentDialogHelper
    {
        private readonly ContentDialog contentDialog;

        private bool isFlyoutOpen = false;

        private bool isPointerEnter = false;

        public bool IsBackgroundPointerDownClosing { get; set; } = true;

        public ContentDialogHelper(ContentDialog contentDialog)
        {
            var isDark = DI.Get<IApplicationThemeListener>().IsDark;

            this.contentDialog = contentDialog;
            this.contentDialog.RequestedTheme = isDark ? ElementTheme.Dark : ElementTheme.Light;
            this.contentDialog.PointerEntered += ContentDialog_PointerEntered;
            this.contentDialog.PointerExited += ContentDialog_PointerExited;

            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
        }

        public void FlyoutOpen()
        {
            isFlyoutOpen = true;
        }

        private void ContentDialog_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isPointerEnter = true;
        }

        private void ContentDialog_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            isPointerEnter = false;
        }

        private void CoreWindow_PointerPressed(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {
            if (!isPointerEnter && !isFlyoutOpen && IsBackgroundPointerDownClosing)
            {
                contentDialog.Hide();
                if (contentDialog is IViewModelContentDialog viewModelDialog)
                {
                    viewModelDialog.ForceClosed();
                }
            }

            isFlyoutOpen = false;
        }
    }
}
