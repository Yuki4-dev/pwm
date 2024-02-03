#nullable enable

using pwm.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class PasswordPage : Page
    {
        public PasswordPageViewModel ViewModel { get; }

        public PasswordPage()
        {
            ViewModel = DI.Get<PasswordPageViewModel>();
            DataContext = ViewModel;

            Resources.Add("ManuFlyoutViewText", ViewModel.Strings.View);
            Resources.Add("ManuFlyoutEditText", ViewModel.Strings.Edit);
            Resources.Add("ManuFlyoutDeleteText", ViewModel.Strings.Delete);

            InitializeComponent();
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Escape)
            {
                ViewModel.EscapeDownCommand.Execute(null);
            }
        }

        private void KeyWordSearchAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (string)args.SelectedItem;
        }

        private void KeyWordSearchAutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyWordSearchAutoSuggestBoxGotFocusAnime.Begin();
        }

        private void KeyWordSearchAutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            KeyWordSearchAutoSuggestBoxLostFocusAnime.Begin();
        }

        private async void RootPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.OnLoadedAsync();
        }
    }
}
