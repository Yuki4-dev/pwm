#nullable enable

using pwm.Core.Models;
using System.Collections.Generic;

namespace pwm.Core.Store
{
    public interface IPasswordInformationEntityStore
    {
        bool Saved { get; }

        PasswordInformationSetting? GetPasswordInformationSetting();

        PasswordInformationEntity[] GetPasswordInformationEntities();

        void Save(PasswordInformationSetting passwordInformationSetting);

        void Initialize(PasswordInformationEntity[] passwordInformationEntities);

        void Initialize(PasswordInformationSetting passwordInformationSetting, PasswordInformationEntity[] passwordInformationEntities);

        void Add(PasswordInformationEntity entity);

        void AddRange(IEnumerable<PasswordInformationEntity> entities);

        bool Remove(PasswordInformationEntity removeEntity);

        bool Update(PasswordInformationEntity oldEntity, PasswordInformationEntity newEntity);
    }
}