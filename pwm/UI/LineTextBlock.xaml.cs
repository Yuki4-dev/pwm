#nullable enable

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace pwm.UI
{
    public sealed partial class LineTextBlock : UserControl
    {
        public static readonly DependencyProperty IsHighlightEnableProperty
            = DependencyProperty.Register(
                                nameof(IsHighlightEnable),
                                typeof(bool),
                                typeof(LineTextBlock),
                                new PropertyMetadata(true, IsHighlightEnableChanged));

        public static readonly DependencyProperty HighlightBrushProperty
            = DependencyProperty.Register(
                                nameof(HighlightBrush),
                                typeof(Brush),
                                typeof(LineTextBlock),
                                PropertyMetadata.Create(CreateHighlightBrush));

        public static readonly DependencyProperty HighlightLengthProperty
            = DependencyProperty.Register(
                                nameof(HighlightLength),
                                typeof(int),
                                typeof(LineTextBlock),
                                new PropertyMetadata(0, HighlightChanged));

        public static readonly DependencyProperty HighlightStartProperty
           = DependencyProperty.Register(
                               nameof(HighlightStart),
                               typeof(int),
                               typeof(LineTextBlock),
                               new PropertyMetadata(0, HighlightChanged));

        public static readonly DependencyProperty TextProperty
           = DependencyProperty.Register(
                               nameof(Text),
                               typeof(string),
                               typeof(LineTextBlock),
                               new PropertyMetadata(string.Empty, TextChanged));

        public bool IsHighlightEnable
        {
            get => (bool)GetValue(IsHighlightEnableProperty);
            set => SetValue(IsHighlightEnableProperty, value);
        }

        public Brush HighlightBrush
        {
            get => (Brush)GetValue(HighlightBrushProperty);
            set => SetValue(HighlightBrushProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public int HighlightStart
        {
            get => (int)GetValue(HighlightStartProperty);
            set => SetValue(HighlightStartProperty, value);
        }

        public int HighlightLength
        {
            get => (int)GetValue(HighlightLengthProperty);
            set => SetValue(HighlightLengthProperty, value);
        }

        public static object CreateHighlightBrush()
        {
            return new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColorLight1"]);
        }

        public LineTextBlock()
        {
            InitializeComponent();
        }

        private static void IsHighlightEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var enable = (e.NewValue as bool?) == true;
            var text = (LineTextBlock)d;
            if (enable)
            {
                text.HighlightChanged();
            }
            else
            {
                text.HighlightingTextBlock.TextHighlighters.Clear();
            }
        }

        private static void HighlightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is int newValue)
            {
                if (newValue < 0)
                {
                    throw new ArgumentException($"value is {newValue}");
                }

                var text = (LineTextBlock)d;
                text.HighlightChanged();
            }
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var text = (LineTextBlock)d;
            text.TextContent.Text = e.NewValue?.ToString() ?? string.Empty;
            text.HighlightChanged();
        }

        private void HighlightChanged()
        {
            if (!IsHighlightEnable)
            {
                return;
            }

            HighlightingTextBlock.TextHighlighters.Clear();
            if (HighlightStart + HighlightLength <= 0)
            {
                return;
            }

            if (HighlightStart + HighlightLength > TextContent.Text.Length)
            {
                return;
            }

            var textRange = new TextRange()
            {
                StartIndex = HighlightStart,
                Length = HighlightLength
            };

            var highlighter = new TextHighlighter()
            {
                Background = HighlightBrush,
                Ranges = { textRange }
            };
            HighlightingTextBlock.TextHighlighters.Add(highlighter);
        }
    }
}
