#nullable enable

using pwm.Core.Models;

namespace pwm.Models.Core.Password
{
    public class PasswordInformationSort
    {
        public SortType Type { get; }

        public PasswordInformationSortTarget Target { get; }

        public PasswordInformationSort(SortType type, PasswordInformationSortTarget target) { Type = type; Target = target; }
    }

    public enum PasswordInformationSortTarget
    {
        WebSiteName, WebSiteURI, UserId, UpdateDate
    }

    public enum SortType
    {
        Asc, Desc
    }

    public static class PasswordInformationSortTargetExtension
    {
        public static string GetValue(this PasswordInformationSortTarget src, PasswordInformationEntity information)
        {
            return src switch
            {
                PasswordInformationSortTarget.WebSiteName => information.WebSiteName,
                PasswordInformationSortTarget.WebSiteURI => information.WebSiteURI,
                PasswordInformationSortTarget.UserId => information.UserId,
                PasswordInformationSortTarget.UpdateDate => information.UpdateDate.ToString(),
                _ => string.Empty,
            };
        }
    }
}
