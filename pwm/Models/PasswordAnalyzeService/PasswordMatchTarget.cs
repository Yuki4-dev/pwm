#nullable enable

using pwm.Core.Models;

namespace pwm.Models.PasswordAnalyzeService
{
    public enum PasswordMatchTarget
    {
        WebSiteName, WebSiteURI, Password, UserId, Description
    }

    public static class PasswordMatchTargetExtension
    {
        public static string Get(this PasswordMatchTarget src, PasswordInformationEntity entity)
        {
            return src switch
            {
                PasswordMatchTarget.WebSiteName => entity.WebSiteName,
                PasswordMatchTarget.WebSiteURI => entity.WebSiteURI,
                PasswordMatchTarget.Password => entity.Password,
                PasswordMatchTarget.UserId => entity.UserId,
                PasswordMatchTarget.Description => entity.Description,
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
