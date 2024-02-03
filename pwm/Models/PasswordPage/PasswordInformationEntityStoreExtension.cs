#nullable enable

using pwm.Core.Models;
using pwm.Core.Store;
using System.Linq;

namespace pwm.Models.PasswordPage
{
    public static class PasswordInformationEntityStoreExtension
    {
        public static PasswordInformation[] ConvertPasswordInformation(this IPasswordInformationEntityStore store, PasswordDisplayStatus passwordDisplayStatus)
        {
            return ConvertPasswordInformation(store.GetPasswordInformationEntities(), passwordDisplayStatus);
        }

        public static PasswordInformation[] ConvertPasswordInformation(PasswordInformationEntity[] entities, PasswordDisplayStatus passwordDisplayStatus)
        {
            return entities.Select(p =>
            {
                var pInfo = PasswordInformation.FromEntity(p);
                pInfo.Password.Status = passwordDisplayStatus;
                return pInfo;
            }).ToArray();
        }
    }
}
