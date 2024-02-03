#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace pwm.Models.PasswordPage
{
    [INotifyPropertyChanged]
    public partial class DisplayPassword
    {
        public const string MASK = "●●●●";

        [ObservableProperty]
        private PasswordDisplayStatus status = PasswordDisplayStatus.Mask;
        partial void OnStatusChanged(PasswordDisplayStatus value)
        {
            OnPropertyChanged(nameof(Text));
        }

        private string password = string.Empty;
        public string Text
        {
            get => Status == PasswordDisplayStatus.Mask ? MASK : password;
            set
            {
                if (Status == PasswordDisplayStatus.Mask)
                {
                    throw new InvalidOperationException($"Status -> {Status}.");
                }
                _ = SetProperty(ref password, value);
            }
        }

        public DisplayPassword(string password, PasswordDisplayStatus status = PasswordDisplayStatus.Mask)
        {
            Status = status;
            this.password = password;
        }

        public string GetPassword()
        {
            return password;
        }

        public override bool Equals(object? obj)
        {
            return obj is DisplayPassword password &&
                   Status == password.Status &&
                   this.password == password.password;
        }

        public override int GetHashCode()
        {
            int hashCode = -707125633;
            hashCode = (hashCode * -1521134295) + Status.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(password);
            return hashCode;
        }
    }

    public enum PasswordDisplayStatus
    {
        Mask, Display
    }
}
