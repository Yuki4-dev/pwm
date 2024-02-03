#nullable enable

using System;
using System.Collections.Generic;

namespace pwm.Core.Models
{
    public class PasswordInformationEntity
    {
        public string WebSiteName { get;  } = string.Empty;

        public string WebSiteURI { get; } = string.Empty;

        public string UserId { get; } = string.Empty;

        public string Password { get; } = string.Empty;

        public string Description { get; } = string.Empty;

        public DateTime UpdateDate { get; } = DateTime.Now;

        public PasswordInformationEntity(string webSiteName, string webSiteURI, string userId, string password, string description, DateTime updateDate)
        {
            WebSiteName = webSiteName;
            WebSiteURI = webSiteURI;
            UserId = userId;
            Password = password;
            Description = description;
            UpdateDate = updateDate;
        }

        public override bool Equals(object obj)
        {
            return obj is PasswordInformationEntity entity &&
                   WebSiteName == entity.WebSiteName &&
                   WebSiteURI == entity.WebSiteURI &&
                   UserId == entity.UserId &&
                   Password == entity.Password &&
                   Description == entity.Description &&
                   UpdateDate == entity.UpdateDate;
        }

        public override int GetHashCode()
        {
            int hashCode = 1464472248;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(WebSiteName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(WebSiteURI);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(UserId);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = (hashCode * -1521134295) + UpdateDate.GetHashCode();
            return hashCode;
        }
    }
}
