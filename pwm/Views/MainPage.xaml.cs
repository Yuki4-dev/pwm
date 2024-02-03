#nullable enable

using CommunityToolkit.Mvvm.Messaging;
using pwm.Core.Models;
using pwm.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace pwm.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page, IRecipient<NavigationPageMessage>
    {
        public string AppName => pwm.Language.Instance.AppName;

        public MainPage()
        {
            InitializeComponent();

            DI.Get<IApplicationThemeListener>().SetTitleBar(AppTitleBar);
            WeakReferenceMessenger.Default.Register(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _ = ContentFarame.Navigate(typeof(PasswordPage));
            base.OnNavigatedTo(e);
        }

        void IRecipient<NavigationPageMessage>.Receive(NavigationPageMessage message)
        {
            NavigationBackButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            _ = ContentFarame.Navigate(message.PageType, message.Parameter);
        }

        private void NavigationBackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ContentFarame.CanGoBack)
            {
                ContentFarame.GoBack();
                NavigationBackButton.Visibility = ContentFarame.CanGoBack ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
