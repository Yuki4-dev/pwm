#nullable enable

using pwm.Core.Models;
using pwm.Core.Modules;
using pwm.Models.Core.Password;
using pwm.Models.PasswordAnalyzeService;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pwm.Service
{
    public class PasswordAnalyzeService : IPasswordAnalyzeService
    {
        private readonly IAppSettingProvider appSettingProvider;

        public PasswordAnalyzeService(IAppSettingProvider appSettingProvider)
        {
            this.appSettingProvider = appSettingProvider;
        }

        public async Task<PasswordInformationEntity[]> SortPasswordAsync(PasswordInformationSort[] sorts, PasswordInformationEntity[] passwords)
        {
            var sortPasswordInformation = passwords.ToList();
            await Task.Run(() =>
            {
                foreach (var sort in sorts.Reverse())
                {
                    sortPasswordInformation.Sort((info1, info2) =>
                    {
                        var str1 = sort.Target.GetValue(info1);
                        var str2 = sort.Target.GetValue(info2);
                        var result = Compare(str1, str2);
                        return result == 0 ? 0 : sort.Type == SortType.Asc ? result : result == 1 ? -1 : 1;
                    });
                }
            });

            return sortPasswordInformation.ToArray();
        }

        public AnalyzePasswordResult AnalyzePassword(PasswordInformationEntity password)
        {
            return string.IsNullOrEmpty(password.WebSiteName)
                ? new AnalyzePasswordResult(false, Language.Instance.PasswordAnalyzeService.NameEmpty)
                : string.IsNullOrEmpty(password.Password)
                ? new AnalyzePasswordResult(false, Language.Instance.PasswordAnalyzeService.PasswordEmpty)
                : password.Password.Length < appSettingProvider.PasswordLength
                ? new AnalyzePasswordResult(false, string.Format(Language.Instance.PasswordAnalyzeService.PasswordLength, password.Password.Length, appSettingProvider.PasswordLength))
                : new AnalyzePasswordResult(true);
        }

        public MatchPasswordResult MatchPassword(PasswordInformationEntity password, PasswordAnalyzeOption[] options)
        {
            var resultInformation = new List<PasswordAnalyzeInformation>();
            foreach (var option in options)
            {
                if (IsMatch(option.Target.Get(password), option, out var info))
                {
                    resultInformation.Add(info!);
                }
            }

            return new MatchPasswordResult(resultInformation.Count != 0, resultInformation.ToArray());
        }

        private int Compare(string str1, string str2)
        {
            return appSettingProvider.IsNaturalOrderSort ? NativeStringComparer.Default.Compare(str1, str2) : str1.CompareTo(str2);
        }

        private bool IsMatch(string matchTarget, PasswordAnalyzeOption option, out PasswordAnalyzeInformation? information)
        {
            if (option.MatchType == PasswordMatchType.PartMatch)
            {
                var index = matchTarget.IndexOf(option.Keyword);
                if (index == -1)
                {
                    information = null;
                    return false;
                }

                information = new PasswordAnalyzeInformation(option.Keyword, option.Keyword, index, option.Keyword.Length, option.Target, PasswordMatchType.PartMatch);
                return true;
            }
            else if (option.MatchType == PasswordMatchType.Regex)
            {
                var regMatches = Regex.Matches(matchTarget, option.Keyword);
                if (regMatches.Count == 0)
                {
                    information = null;
                    return false;
                }

                var index = regMatches[0].Index;
                var value = regMatches[0].Value;
                information = new PasswordAnalyzeInformation(option.Keyword, value, index, value.Length, option.Target, PasswordMatchType.PartMatch);
                return true;
            }
            else
            {
                if (matchTarget != option.Keyword)
                {
                    information = null;
                    return false;
                }

                information = new PasswordAnalyzeInformation(option.Keyword, matchTarget, 0, matchTarget.Length, option.Target, PasswordMatchType.PartMatch);
                return true;
            }
        }
    }
}
