#nullable enable

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using pwm.Core.API;
using pwm.Core.Models;
using pwm.Core.Store;
using pwm.Extension;
using pwm.Models.Core.Password;
using pwm.Models.InitializeContentDialog;
using pwm.Models.PasswordDataProvider;
using pwm.Models.PasswordInformationContentDialog;
using pwm.Models.PasswordPage;
using pwm.Service;
using pwm.ViewModels.Dialogs;
using pwm.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace pwm.ViewModels
{
    [ObservableObject]
    public partial class PasswordPageViewModel : IViewModel, IClosingHandler
    {
        public Type View => typeof(PasswordPage);

        public Language.PasswordPageLanguage Strings => Language.Instance.PasswordPage;

        private bool isLoaded = false;

        private readonly SemaphoreSlim semaphore = new(1, 1);

        private readonly Queue<PasswordInformationSortTarget> sortQueue = new();

        private readonly ILogger logger;

        private readonly IAppSettingProvider appSettingProvider;

        private readonly IPasswordInformationEntityStore passwordInformationEntityStore;

        private readonly IApplicationEventManager applicationEventManager;

        private readonly IDialogService dialogService;

        private readonly IPasswordAnalyzeService passwordAnalyzeService;

        private readonly IPasswordDataProvider passwordDataProvider;

        [ObservableProperty]
        private string searchKeyword = string.Empty;

        [ObservableProperty]
        private int displayPasswordCount = 0;

        public bool IsNotBusy => !isBusy;
        [ObservableProperty]
        private bool isBusy = false;
        partial void OnIsBusyChanged(bool value)
        {
            OnPropertyChanged(nameof(IsNotBusy));
        }

        [ObservableProperty]
        private bool isDisplayPassword = false;
        private PasswordDisplayStatus CurrentPasswordDisplayStatus => IsDisplayPassword ? PasswordDisplayStatus.Display : PasswordDisplayStatus.Mask;
        partial void OnIsDisplayPasswordChanged(bool value)
        {
            foreach (var pInfo in DisplayPasswordInformation)
            {
                pInfo.Password.Status = CurrentPasswordDisplayStatus;
            }
        }

        [ObservableProperty]
        private bool checkedWebSiteName = false;
        partial void OnCheckedWebSiteNameChanged(bool value)
        {
            sortQueue.Enqueue(PasswordInformationSortTarget.WebSiteName);
            RunSortTaskAsync();
        }

        [ObservableProperty]
        private bool checkedWebSiteURI = false;
        partial void OnCheckedWebSiteURIChanged(bool value)
        {
            sortQueue.Enqueue(PasswordInformationSortTarget.WebSiteURI);
            RunSortTaskAsync();
        }

        [ObservableProperty]
        private bool checkedUserId = false;
        partial void OnCheckedUserIdChanged(bool value)
        {
            sortQueue.Enqueue(PasswordInformationSortTarget.UserId);
            RunSortTaskAsync();
        }

        [ObservableProperty]
        private bool checkedUpdateDate = false;
        partial void OnCheckedUpdateDateChanged(bool value)
        {
            sortQueue.Enqueue(PasswordInformationSortTarget.UpdateDate);
            RunSortTaskAsync();
        }

        public bool IsSelectedAll
        {
            get => DisplayPasswordInformation.Count != 0 && DisplayPasswordInformation.All(p => p.IsChecked);
            set => DisplayPasswordInformation.ToList().ForEach(p => p.IsChecked = value);
        }

        public ObservableCollectionEx<PasswordInformation> DisplayPasswordInformation { get; } = new();

        public ObservableCollectionEx<string> SuggestWords { get; } = new();

        public PasswordPageViewModel(ILogger logger, IAppSettingProvider appSettingProvider, IPasswordInformationEntityStore passwordInformationEntityStore, IApplicationEventManager applicationEventManager, IDialogService dialogService, IPasswordDataProvider passwordDataProvider, IPasswordAnalyzeService passwordAnalyzeService)
        {
            this.logger = logger;
            this.appSettingProvider = appSettingProvider;
            this.passwordInformationEntityStore = passwordInformationEntityStore;
            this.dialogService = dialogService;
            this.passwordDataProvider = passwordDataProvider;
            this.passwordAnalyzeService = passwordAnalyzeService;
            this.applicationEventManager = applicationEventManager;

            _ = applicationEventManager.RegisterEnteredBackground((_, __) =>
            {
                IsDisplayPassword = false;
            });

            DisplayPasswordInformation.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                DisplayPasswordCount = DisplayPasswordInformation.Count;
            };
        }

        public async Task OnLoadedAsync()
        {
            if (isLoaded)
            {
                return;
            }
            isLoaded = true;

            _ = applicationEventManager.AddClosingHandler(this);

            await BusyInvokeAsync(async () =>
            {
                InitializeContentDialogViewModel viewModel = new();
                _ = await dialogService.ShowContentDialogAsync(viewModel);

                if (viewModel.Result == InitializeContentDialogResult.CreateNew)
                {
                    await SaveAsync();
                }
                else if (viewModel.Result == InitializeContentDialogResult.OpenOne)
                {
                    var context = await passwordDataProvider.OpenAsync();
                    if (context != null)
                    {
                        passwordInformationEntityStore.Initialize(context.PasswordInformationSetting, context.PasswordInformationEntities);
                    }
                }

                Update();
            });
        }

        [RelayCommand]
        private void EscapeDown()
        {
            foreach (PasswordInformation information in DisplayPasswordInformation)
            {
                information.WebSiteName.IsHighlightEnable = false;
                information.WebSiteURI.IsHighlightEnable = false;
                information.UserId.IsHighlightEnable = false;
                information.Description.IsHighlightEnable = false;
            }
        }

        [RelayCommand]
        private async Task AddPassword()
        {
            await BusyInvokeAsync(async () =>
            {
                PasswordInformationContentDialogViewModel viewModel = new(PasswordInformationContentDialogType.Add);
                var result = await dialogService.ShowContentDialogAsync(viewModel);
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                passwordInformationEntityStore.Add(viewModel.GetPasswordInformationEntity());
                Update();
            });
        }

        [RelayCommand]
        private async Task ViewPassword(PasswordInformation selectInformation)
        {
            await BusyInvokeAsync(() =>
            {
                var viewModel = PasswordInformationContentDialogViewModel.FromPasswordInformationEntity(selectInformation.ToEntity(), PasswordInformationContentDialogType.View);
                return dialogService.ShowContentDialogAsync(viewModel);
            });
        }

        [RelayCommand]
        private async Task EditPassword(PasswordInformation selectInformation)
        {
            await BusyInvokeAsync(async () =>
            {
                var selectPasswordEntity = selectInformation.ToEntity();
                var viewModel = PasswordInformationContentDialogViewModel.FromPasswordInformationEntity(selectPasswordEntity, PasswordInformationContentDialogType.Edit);

                var result = await dialogService.ShowContentDialogAsync(viewModel);
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                var newPasswordEntity = viewModel.GetPasswordInformationEntity();
                if (!passwordInformationEntityStore.Update(selectPasswordEntity, newPasswordEntity))
                {
                    _ = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.EditFailedMessage);
                    return;
                }

                Update();
            });
        }

        [RelayCommand]
        private async Task SearchPassword()
        {
            await BusyInvokeAsync(() =>
            {
                Update();
                return Task.CompletedTask;
            });

            var trimKeyword = SearchKeyword.Trim();
            if (!SuggestWords.Contains(trimKeyword))
            {
                SuggestWords.Add(trimKeyword);
            }
        }

        [RelayCommand]
        private async Task DeletePassword(PasswordInformation selectInformation)
        {
            await BusyInvokeAsync(async () =>
            {
                var result = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.GetDeleteMessage(selectInformation));
                if (result == ContentDialogResult.Primary)
                {
                    _ = passwordInformationEntityStore.Remove(selectInformation.ToEntity());
                    Update();
                }
            });
        }

        [RelayCommand]
        private async Task DeleteSelection()
        {
            await BusyInvokeAsync(async () =>
            {
                int selectCount = DisplayPasswordInformation.Where(i => i.IsChecked).Count();
                if (selectCount == 0)
                {
                    _ = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.DeleteSelectionNothingMessage);
                    return;
                }

                var result = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.GetDeleteSelection(selectCount));
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                var removeInformation = DisplayPasswordInformation.Where(p => p.IsChecked).Select(p => p.ToEntity());
                foreach (var information in removeInformation)
                {
                    _ = passwordInformationEntityStore.Remove(information);
                }

                Update();
            });
        }

        [RelayCommand]
        private async Task Save()
        {
            await BusyInvokeAsync(() =>
            {
                return SaveAsync();
            });
        }

        [RelayCommand]
        private async Task Export()
        {
            await BusyInvokeAsync(() =>
            {
                return passwordDataProvider.ExportAsync(passwordInformationEntityStore.GetPasswordInformationEntities());
            });
        }

        [RelayCommand]
        private void Setting()
        {
            _ = WeakReferenceMessenger.Default.Send(new NavigationPageMessage(typeof(SettingPage)));
        }

        [RelayCommand]
        private async void GoUri(PasswordInformation selectInformation)
        {
            var selectedUri = selectInformation.WebSiteURI.Text;
            if (Uri.TryCreate(selectedUri, UriKind.Absolute, out Uri? uri))
            {
                _ = await Windows.System.Launcher.LaunchUriAsync(uri);
            }
            else
            {
                _ = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.GetInvalidURLFormat(selectedUri));
            }
        }

        private async Task BusyInvokeAsync(Func<Task> invoker)
        {
            await semaphore.WaitAsync();
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            _ = semaphore.Release();

            try
            {
                await invoker();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
                _ = await dialogService.ShowErrorMessageDialogAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            var setting = passwordInformationEntityStore.GetPasswordInformationSetting();

            PasswordDataSaveResult result;
            if (setting == null)
            {
                result = await passwordDataProvider.CreateAsync(passwordInformationEntityStore.GetPasswordInformationEntities());
            }
            else
            {
                result = await passwordDataProvider.SaveAsync(passwordInformationEntityStore.GetPasswordInformationEntities(), setting);
            }

            if (result.IsDone)
            {
                passwordInformationEntityStore.Save(result.PasswordDataContext!.PasswordInformationSetting);
            }
        }

        // Public For Test
        public void Update()
        {
            EscapeDown();
            DisplayPasswordInformation.Clear();

            var passwordInformation = passwordInformationEntityStore.ConvertPasswordInformation(CurrentPasswordDisplayStatus);
            string trimSearchKeyword = SearchKeyword.Trim();
            if (!string.IsNullOrEmpty(trimSearchKeyword))
            {
                var searchInformation = SearchPasswordInformation(passwordInformation, trimSearchKeyword);
                DisplayPasswordInformation.AddRange(searchInformation);
            }
            else
            {
                DisplayPasswordInformation.AddRange(passwordInformation);
            }

            OnPropertyChanged(nameof(IsSelectedAll));
        }

        private async void RunSortTaskAsync()
        {
            await semaphore.WaitAsync();
            while (sortQueue.TryDequeue(out PasswordInformationSortTarget target))
            {
                await SortAsync(new PasswordInformationSortTarget[] { target });
            }
            _ = semaphore.Release();
        }

        private async Task SortAsync(PasswordInformationSortTarget[] targets)
        {
            if (DisplayPasswordInformation.Count < 2)
            {
                return;
            }

            var sorts = targets.Select(t =>
            {
                bool isAsc = t switch
                {
                    PasswordInformationSortTarget.WebSiteName => CheckedWebSiteName,
                    PasswordInformationSortTarget.WebSiteURI => CheckedWebSiteURI,
                    PasswordInformationSortTarget.UserId => CheckedUserId,
                    PasswordInformationSortTarget.UpdateDate => CheckedUpdateDate,
                    _ => throw new NotImplementedException(),
                };
                return new PasswordInformationSort(isAsc ? SortType.Asc : SortType.Desc, t);
            }).ToArray();

            var sortPasswordEntity = await passwordAnalyzeService.SortPasswordAsync(sorts, DisplayPasswordInformation.Select(p => p.ToEntity()).ToArray());
            DisplayPasswordInformation.Clear();
            DisplayPasswordInformation.AddRange(PasswordInformationEntityStoreExtension.ConvertPasswordInformation(sortPasswordEntity, CurrentPasswordDisplayStatus));

            OnPropertyChanged(nameof(IsSelectedAll));
        }

        private PasswordInformation[] SearchPasswordInformation(PasswordInformation[] passwordInformation, string searchedKeyWord)
        {
            return passwordInformation.AsParallel()
            .Where((information) =>
            {
                var compOption = new ComputePasswordAnalyzeOption(appSettingProvider.PasswordSearchTargetSetting, searchedKeyWord);
                var result = passwordAnalyzeService.MatchPassword(information.ToEntity(), compOption.Options);
                if (!result.IsMatch)
                {
                    return false;
                }
                else
                {
                    information.ApplyPasswordAnalyzeInformation(result.Matches);
                    return true;
                }

            }).ToArray();
        }

        public bool Handled()
        {
            if (passwordInformationEntityStore.Saved)
            {
                return false;
            }

            ShowClosingContentDialogAsync();
            return true;
        }

        private async void ShowClosingContentDialogAsync()
        {
            dialogService.CloseDialog();

            var result = await dialogService.ShowMessageDialogAsync(PasswordPageShowMessages.ClosingMessage);
            if (result == ContentDialogResult.None)
            {
                return;
            }

            if (result == ContentDialogResult.Primary)
            {
                IsBusy = true;
                try
                {
                    await SaveAsync();
                }
                catch (Exception ex)
                {
                    _ = await dialogService.ShowErrorMessageDialogAsync(ex);
                    return;
                }
                finally
                {
                    IsBusy = false;
                }
            }

            applicationEventManager.Exit();
        }
    }
}