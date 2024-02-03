#nullable enable

using pwm.Core.Models;
using pwm.Models.PasswordDataProvider;
using System.Threading.Tasks;

namespace pwm.Service
{
    public interface IPasswordDataProvider
    {
        Task<PasswordDataContext?> OpenAsync();

        Task<PasswordDataSaveResult> CreateAsync(PasswordInformationEntity[] passwordInformation);

        Task<PasswordDataSaveResult> SaveAsync(PasswordInformationEntity[] passwordInformation, PasswordInformationSetting passwordInformationSetting);

        Task<PasswordDataSaveResult> ExportAsync(PasswordInformationEntity[] passwordInformation);
    }
}
