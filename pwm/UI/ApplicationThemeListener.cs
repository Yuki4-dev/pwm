#nullable enable

using pwm.Models.Setting;
using pwm.Service;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace pwm.UI
{
    public class ApplicationThemeListener : IApplicationThemeListener
    {
        private readonly UISettings uISettings = new();

        private readonly CoreDispatcher uiDispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

        private readonly IAppSettingProvider appSettingProvider;

        public bool IsDark => appSettingProvider.ApplicationTheme == PwmApplicationTheme.Dark
                   || (appSettingProvider.ApplicationTheme == PwmApplicationTheme.System && Application.Current.RequestedTheme == ApplicationTheme.Dark);

        public ApplicationThemeListener(IAppSettingProvider settingProvider)
        {
            appSettingProvider = settingProvider;
            appSettingProvider.SettingChanged += SettingProvider_SettingChanged;
            uISettings.ColorValuesChanged += UISettings_ColorValuesChanged;
        }

        private void SettingProvider_SettingChanged(object sender, System.EventArgs e)
        {
            _ = uiDispatcher.RunIdleAsync((e) =>
            {
                ApplyTheme();
            });
        }

        private void UISettings_ColorValuesChanged(UISettings sender, object args)
        {
            _ = uiDispatcher.RunIdleAsync((e) =>
            {
                ApplyTheme();
            });
        }

        public void SetTitleBar(UIElement titleBar)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(titleBar);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (Window.Current.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = IsDark ? ElementTheme.Dark : ElementTheme.Light;
            }

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (IsDark)
            {
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 90, 90, 90);
                titleBar.ButtonPressedForegroundColor = Colors.White;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 120, 120, 120);
                titleBar.InactiveForegroundColor = Colors.Gray;
                titleBar.InactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.BackgroundColor = Color.FromArgb(255, 45, 45, 45);
            }
            else
            {
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonHoverForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 180, 180, 180);
                titleBar.ButtonPressedForegroundColor = Colors.Black;
                titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 150, 150, 150);
                titleBar.InactiveForegroundColor = Colors.DimGray;
                titleBar.InactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.DimGray;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.BackgroundColor = Color.FromArgb(255, 210, 210, 210);
            }
        }
    }
}
