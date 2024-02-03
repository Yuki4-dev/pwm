#nullable enable

using pwm.ViewModels;
using Windows.UI.Xaml.Controls;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace pwm.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPageViewModel ViewModel { get; }

        public SettingPage()
        {
            ViewModel = DI.Get<SettingPageViewModel>();
            DataContext = ViewModel;

            Resources.Add("ExpanderSearchPasswordTargetText", ViewModel.Strings.SearchPasswordTarget);
            Resources.Add("ExpanderAboutText", ViewModel.Strings.About);

            InitializeComponent();
        }
    }
}
