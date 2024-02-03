#nullable enable

using pwm.Core.Models;
using pwm.Models.Core.Password;
using pwm.Models.PasswordAnalyzeService;
using System.Threading.Tasks;

namespace pwm.Service
{
    public interface IPasswordAnalyzeService
    {
        public Task<PasswordInformationEntity[]> SortPasswordAsync(PasswordInformationSort[] sorts, PasswordInformationEntity[] passwords);

        public AnalyzePasswordResult AnalyzePassword(PasswordInformationEntity password);

        public MatchPasswordResult MatchPassword(PasswordInformationEntity password, PasswordAnalyzeOption[] options);
    }
}