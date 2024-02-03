#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace pwm.Models.PasswordPage
{
    [INotifyPropertyChanged]
    public partial class HighlightInformation
    {
        [ObservableProperty]
        private bool isHighlightEnable = true;

        [ObservableProperty]
        private string text = string.Empty;

        [ObservableProperty]
        private int index = 0;

        [ObservableProperty]
        private int length = 0;

        public override bool Equals(object obj)
        {
            return obj is HighlightInformation information &&
                   Text == information.Text &&
                   Index == information.Index &&
                   Length == information.Length;
        }

        public override int GetHashCode()
        {
            int hashCode = -972546954;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = (hashCode * -1521134295) + Index.GetHashCode();
            hashCode = (hashCode * -1521134295) + Length.GetHashCode();
            return hashCode;
        }
    }
}
