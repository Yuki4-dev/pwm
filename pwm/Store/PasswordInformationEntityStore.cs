#nullable enable

using pwm.Core.Models;
using pwm.Core.Store;
using System.Collections.Generic;
using System.Linq;

namespace pwm.Store
{
    public class PasswordInformationEntityStore : IPasswordInformationEntityStore
    {
        public bool Saved { get; private set; } = false;

        private PasswordInformationSetting? passwordInformationSetting;

        private readonly List<PasswordInformationEntity> passwordInformationEntities = new();

        public PasswordInformationSetting? GetPasswordInformationSetting()
        {
            return passwordInformationSetting;
        }

        public PasswordInformationEntity[] GetPasswordInformationEntities()
        {
            return passwordInformationEntities.ToArray();
        }

        public void Initialize(PasswordInformationEntity[] passwordInformationEntities)
        {
            InternalInitialize(passwordInformationEntities);
        }

        public void Initialize(PasswordInformationSetting passwordInformationSetting, PasswordInformationEntity[] passwordInformationEntities)
        {
            InternalInitialize(passwordInformationEntities, passwordInformationSetting);
        }

        private void InternalInitialize(PasswordInformationEntity[] passwordInformationEntities, PasswordInformationSetting? passwordInformationSetting = null)
        {
            Saved = false;
            if (passwordInformationSetting != null)
            {
                Save(passwordInformationSetting);
            }

            this.passwordInformationEntities.Clear();
            this.passwordInformationEntities.AddRange(passwordInformationEntities);
        }

        public void Save(PasswordInformationSetting passwordInformationSetting)
        {
            Saved = true;
            this.passwordInformationSetting = passwordInformationSetting;
        }

        public void Add(PasswordInformationEntity entity)
        {
            Saved = false;
            passwordInformationEntities.Add(entity);
        }

        public void AddRange(IEnumerable<PasswordInformationEntity> entities)
        {
            Saved = false;
            passwordInformationEntities.AddRange(entities);
        }

        public bool Remove(PasswordInformationEntity removeEntity)
        {
            Saved = false;
            return passwordInformationEntities.Remove(removeEntity);
        }

        public bool Update(PasswordInformationEntity oldEntity, PasswordInformationEntity newEntity)
        {
            var targetEntity = passwordInformationEntities.FirstOrDefault(e => e.Equals(oldEntity));
            if (targetEntity == null)
            {
                return false;
            }

            var index = passwordInformationEntities.IndexOf(targetEntity);
            if (index == -1 || !Remove(targetEntity))
            {
                return false;
            }

            Saved = false;
            passwordInformationEntities.Insert(index, newEntity);
            return true;
        }
    }
}
