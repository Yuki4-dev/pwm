#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using pwm.Core.Models;
using pwm.Models.PasswordAnalyzeService;
using System;
using System.Collections.Generic;

namespace pwm.Models.PasswordPage
{
    [INotifyPropertyChanged]
    public partial class PasswordInformation
    {
        [ObservableProperty]
        private bool isChecked = false;

        [ObservableProperty]
        private DisplayPassword password = new(string.Empty);

        [ObservableProperty]
        private HighlightInformation webSiteName = new();

        [ObservableProperty]
        private HighlightInformation webSiteURI = new();

        [ObservableProperty]
        private HighlightInformation userId = new();

        [ObservableProperty]
        private HighlightInformation description = new();

        [ObservableProperty]
        private DateTime updateDate = DateTime.Now;

        public PasswordInformation() { }

        public PasswordInformation(DisplayPassword password, string webSiteName, string webSiteURI, string userId, string description, DateTime? updateDate = null)
        {
            this.password = password;
            this.webSiteName.Text = webSiteName;
            this.webSiteURI.Text = webSiteURI;
            this.userId.Text = userId;
            this.description.Text = description;
            this.updateDate = updateDate ?? DateTime.Now;
        }

        public static PasswordInformation FromEntity(PasswordInformationEntity entity)
        {
            return new PasswordInformation(new DisplayPassword(entity.Password),
                                            entity.WebSiteName,
                                            entity.WebSiteURI,
                                            entity.UserId,
                                            entity.Description,
                                            entity.UpdateDate);
        }

        public PasswordInformationEntity ToEntity()
        {
            return new PasswordInformationEntity(
                password: Password.GetPassword(),
                webSiteName: WebSiteName.Text,
                webSiteURI: WebSiteURI.Text,
                userId: UserId.Text,
                description: Description.Text,
                updateDate: UpdateDate
            );
        }

        public void ApplyPasswordAnalyzeInformation(IEnumerable<PasswordAnalyzeInformation> passwordAnalyzes)
        {
            foreach (var passwordAnalyze in passwordAnalyzes)
            {
                if (passwordAnalyze.Target == PasswordMatchTarget.WebSiteName)
                {
                    WebSiteName.IsHighlightEnable = true;
                    WebSiteName.Index = passwordAnalyze.Index;
                    WebSiteName.Length = passwordAnalyze.Length;
                }
                else if (passwordAnalyze.Target == PasswordMatchTarget.WebSiteURI)
                {
                    WebSiteURI.IsHighlightEnable = true;
                    WebSiteURI.Index = passwordAnalyze.Index;
                    WebSiteURI.Length = passwordAnalyze.Length;
                }
                else if (passwordAnalyze.Target == PasswordMatchTarget.UserId)
                {
                    UserId.IsHighlightEnable = true;
                    UserId.Index = passwordAnalyze.Index;
                    UserId.Length = passwordAnalyze.Length;
                }
            }
        }

        public override string ToString()
        {
            return $"{WebSiteName.Text} ({WebSiteURI.Text})";
        }

        public override bool Equals(object? obj)
        {
            return obj is PasswordInformation information &&
                   IsChecked == information.IsChecked &&
                   EqualityComparer<DisplayPassword>.Default.Equals(Password, information.Password) &&
                   EqualityComparer<HighlightInformation>.Default.Equals(WebSiteName, information.WebSiteName) &&
                   EqualityComparer<HighlightInformation>.Default.Equals(WebSiteURI, information.WebSiteURI) &&
                   EqualityComparer<HighlightInformation>.Default.Equals(UserId, information.UserId) &&
                   EqualityComparer<HighlightInformation>.Default.Equals(description, information.description) &&
                   EqualityComparer<HighlightInformation>.Default.Equals(Description, information.Description) &&
                   UpdateDate == information.UpdateDate;
        }

        public override int GetHashCode()
        {
            int hashCode = 2039795758;
            hashCode = (hashCode * -1521134295) + IsChecked.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<DisplayPassword>.Default.GetHashCode(Password);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HighlightInformation>.Default.GetHashCode(WebSiteName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HighlightInformation>.Default.GetHashCode(WebSiteURI);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HighlightInformation>.Default.GetHashCode(UserId);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HighlightInformation>.Default.GetHashCode(description);
            hashCode = (hashCode * -1521134295) + EqualityComparer<HighlightInformation>.Default.GetHashCode(Description);
            hashCode = (hashCode * -1521134295) + UpdateDate.GetHashCode();
            return hashCode;
        }
    }
}
