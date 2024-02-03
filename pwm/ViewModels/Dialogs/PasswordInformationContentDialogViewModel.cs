#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using pwm.Core.Models;
using pwm.Models.PasswordInformationContentDialog;
using pwm.Service;
using pwm.Views.Dialogs;
using System;
using System.Collections.Generic;

namespace pwm.ViewModels.Dialogs
{
    [INotifyPropertyChanged]
    public partial class PasswordInformationContentDialogViewModel : IViewModel
    {
        public Type View => typeof(PasswordInformationContentDialog);

        public Language.PasswordInformationContentDialogLanguage Strings => Language.Instance.PasswordInformationContentDialog;

        private readonly IList<string> analyzePropertyName = new List<string>
        {
                nameof(WebSiteName),nameof(WebSiteURI),nameof(UserId),nameof(Password),nameof(Description)
        };

        private readonly IPasswordAnalyzeService passwordAnalyzeService;

        private PasswordInformationContentDialogType passwordInformationContentDialogType = PasswordInformationContentDialogType.View;
        public PasswordInformationContentDialogType PasswordInformationContentDialogType
        {
            get => passwordInformationContentDialogType;
            set
            {
                if (SetProperty(ref passwordInformationContentDialogType, value))
                {
                    ChangedPasswordInformationContentDialogType();
                }
            }
        }

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string webSiteName = string.Empty;

        [ObservableProperty]
        private string webSiteURI = string.Empty;

        [ObservableProperty]
        private string userId = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string updateDate = DateTime.Now.ToString();

        private string icon = string.Empty;
        public string Icon
        {
            get => icon;
            private set => SetProperty(ref icon, value);
        }

        private string title = string.Empty;
        public string Title
        {
            get => title;
            private set => SetProperty(ref title, value);
        }

        private bool canCommit = false;
        public bool CanCommit
        {
            get => canCommit && !IsReadOnly && !HiddenPassword;
            private set => SetProperty(ref canCommit, value);
        }

        private bool hiddenPassword = true;
        public bool HiddenPassword
        {
            get => hiddenPassword;
            private set
            {
                if (SetProperty(ref hiddenPassword, value))
                {
                    OnPropertyChanged(nameof(CanCommit));
                }
            }
        }

        private bool isReadOnly = true;
        public bool IsReadOnly
        {
            get => isReadOnly;
            private set
            {
                if (SetProperty(ref isReadOnly, value))
                {
                    OnPropertyChanged(nameof(CanCommit));
                }
            }
        }

        public PasswordInformationContentDialogViewModel(PasswordInformationContentDialogType passwordInformationContentDialogType)
        {
            passwordAnalyzeService = DI.Get<IPasswordAnalyzeService>();
            this.passwordInformationContentDialogType = passwordInformationContentDialogType;

            PropertyChanged += PasswordInformationContentDialogViewModel_PropertyChanged;

            ChangedPasswordInformationContentDialogType();
        }

        private void PasswordInformationContentDialogViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UpdateDate))
            {
                return;
            }
            UpdateDate = DateTime.Now.ToString();

            if (analyzePropertyName.Contains(e.PropertyName))
            {
                var result = passwordAnalyzeService.AnalyzePassword(GetPasswordInformationEntity());
                ErrorMessage = result.ErrorMessage;
                CanCommit = result.Success;
            }
        }

        private void ChangedPasswordInformationContentDialogType()
        {
            if (PasswordInformationContentDialogType == PasswordInformationContentDialogType.Add)
            {
                IsReadOnly = false;
                HiddenPassword = false;
                Icon = "NewFolder";
                Title = Strings.TitleAdd;
            }
            else if (PasswordInformationContentDialogType == PasswordInformationContentDialogType.View)
            {
                IsReadOnly = true;
                HiddenPassword = true;
                Icon = "PreviewLink";
                Title = Strings.TitleView;
            }
            else if (PasswordInformationContentDialogType == PasswordInformationContentDialogType.Edit)
            {
                IsReadOnly = false;
                HiddenPassword = false;
                Icon = "EditPassword";
                Title = Strings.TitleEdit;
            }
        }

        public PasswordInformationEntity GetPasswordInformationEntity()
        {
            return new PasswordInformationEntity(
                password: Password,
                webSiteName: WebSiteName,
                webSiteURI: WebSiteURI,
                userId: UserId,
                description: Description,
                updateDate: DateTime.Parse(UpdateDate)
            );

        }

        public static PasswordInformationContentDialogViewModel FromPasswordInformationEntity(PasswordInformationEntity entity, PasswordInformationContentDialogType passwordInformationContentDialogType)
        {
            return new PasswordInformationContentDialogViewModel(passwordInformationContentDialogType)
            {
                Password = entity.Password,
                WebSiteName = entity.WebSiteName,
                WebSiteURI = entity.WebSiteURI,
                UserId = entity.UserId,
                Description = entity.Description,
                UpdateDate = entity.UpdateDate.ToString(),
            };
        }
    }
}
