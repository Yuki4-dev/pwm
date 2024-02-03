#nullable enable

using Newtonsoft.Json;
using pwm.Core.Crypto;
using pwm.Core.Models;
using pwm.Models.PasswordDataProvider;
using pwm.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace pwm.Service
{
    public class PasswordDataProvider : IPasswordDataProvider
    {
        private const string OUTPUT_FILE_DATE_PATTERN = "yyyy-MM-dd-HH";

        private readonly IDialogService dialogService;

        private readonly IFileAccessService fileAccessService;

        private readonly ICryptographyLogic cryptographyLogic;

        private readonly IEnumerable<string> filters = new List<string>() { ".txt" };

        private readonly IDictionary<string, IList<string>> choices = new Dictionary<string, IList<string>>()
        {
               {"txt", new List<string>() { ".txt" } }
        };

        public PasswordDataProvider(IDialogService dialogService, IFileAccessService fileAccessService, ICryptographyLogic cryptographyLogic)
        {
            this.dialogService = dialogService;
            this.fileAccessService = fileAccessService;
            this.cryptographyLogic = cryptographyLogic;
        }

        public async Task<PasswordDataContext?> OpenAsync()
        {
            var openFile = await fileAccessService.OpenAsync(filters);
            if (openFile == null)
            {
                return null;
            }

            var viewModel = new InputPasswordContentDialogViewModel();
            if (ContentDialogResult.Primary != await dialogService.ShowContentDialogAsync(viewModel)
                || string.IsNullOrEmpty(viewModel.Password))
            {
                return null;
            }

            var stringInformation = await fileAccessService.ReadAsync(openFile);
            var loadInformation = await DeCryptographyStringAsync(stringInformation, viewModel.Password);
            return loadInformation == null
                ? null
                : new PasswordDataContext(new PasswordInformationSetting(openFile, viewModel.Password), loadInformation);
        }


        public async Task<PasswordDataSaveResult> CreateAsync(PasswordInformationEntity[] passwordInformation)
        {
            var passwordInformationSetting = await CreatePasswordFileAsync();
            if (passwordInformationSetting == null)
            {
                return PasswordDataSaveResult.Canceled;
            }

            await InternalSaveAsync(passwordInformation, passwordInformationSetting);
            return new PasswordDataSaveResult(true, new PasswordDataContext(passwordInformationSetting, passwordInformation.ToArray()));
        }

        public async Task<PasswordDataSaveResult> SaveAsync(PasswordInformationEntity[] passwordInformation, PasswordInformationSetting passwordInformationSetting)
        {
            await InternalSaveAsync(passwordInformation, passwordInformationSetting);
            return new PasswordDataSaveResult(true, new PasswordDataContext(passwordInformationSetting, passwordInformation));
        }

        public async Task<PasswordDataSaveResult> ExportAsync(PasswordInformationEntity[] passwordInformation)
        {
            var setting = await CreatePasswordFileAsync();
            if (setting == null)
            {
                return PasswordDataSaveResult.Canceled;
            }

            await InternalSaveAsync(passwordInformation, setting);
            return new PasswordDataSaveResult(true, new PasswordDataContext(setting, passwordInformation));
        }

        private async Task<PasswordInformationSetting?> CreatePasswordFileAsync()
        {
            var viewModel = new CreatePasswordContentDialogViewModel();
            if (ContentDialogResult.Primary != await dialogService.ShowContentDialogAsync(viewModel))
            {
                return null;
            }
            if (!viewModel.CanUse)
            {
                return null;
            }

            var fileName = "data-" + DateTime.Now.ToString(OUTPUT_FILE_DATE_PATTERN);
            var file = await fileAccessService.SaveAsync(fileName, choices);
            return file == null ? null : new PasswordInformationSetting(file, viewModel.GetPassword());
        }

        private async Task InternalSaveAsync(PasswordInformationEntity[] passwordInformation, PasswordInformationSetting setting)
        {
            var stringInformation = await EnCryptographyToStringAsync(passwordInformation, setting.Password);
            await fileAccessService.WriteAsync(setting.OpenFile, stringInformation ?? "");
        }

        private async Task<PasswordInformationEntity[]?> DeCryptographyStringAsync(string base64Data, string key)
        {
            return await Task.Run(() =>
            {
                var jsonData = cryptographyLogic.DeCryptography(base64Data, key);
                return JsonConvert.DeserializeObject<PasswordInformationEntity[]>(jsonData);
            });
        }

        private async Task<string?> EnCryptographyToStringAsync(PasswordInformationEntity[] passwordInformation, string password)
        {
            return await Task.Run(() =>
            {
                var jsonData = JsonConvert.SerializeObject(passwordInformation, Formatting.Indented);
                return cryptographyLogic.EnCryptography(jsonData, password);
            });
        }
    }
}
