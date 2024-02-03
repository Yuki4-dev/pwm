#nullable enable

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace pwm.UI
{
    [ContentProperty(Name = nameof(SettingActionableElement))]
    public sealed partial class ExpandableControl : UserControl
    {
        public FrameworkElement? SettingActionableElement { get; set; }

        public static readonly DependencyProperty TitleProperty
                    = DependencyProperty.Register(
                        nameof(Title),
                        typeof(string),
                        typeof(ExpandableControl),
                        new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty
                    = DependencyProperty.Register(
                        nameof(Description),
                        typeof(string),
                        typeof(ExpandableControl),
                        new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty IconProperty
                    = DependencyProperty.Register(
                        nameof(Icon),
                        typeof(IconElement),
                        typeof(ExpandableControl),
                        new PropertyMetadata(null));

        public IconElement Icon
        {
            get => (IconElement)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ExpandableControl()
        {
            InitializeComponent();
        }

        private void MainPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width == e.PreviousSize.Width || ActionableElement == null)
            {
                return;
            }

            _ = ActionableElement.ActualWidth > e.NewSize.Width / 3
                ? VisualStateManager.GoToState(this, "CompactState", false)
                : VisualStateManager.GoToState(this, "NormalState", false);
        }
    }
}
