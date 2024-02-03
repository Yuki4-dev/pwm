#nullable enable

using Windows.UI.Xaml;

namespace pwm.UI
{
    public interface IApplicationThemeListener
    {
        public bool IsDark { get; }

        public void ApplyTheme();

        public void SetTitleBar(UIElement titleBar);
    }
}
