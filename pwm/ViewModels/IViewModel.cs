#nullable enable

using System;

namespace pwm.ViewModels
{
    public interface IViewModel
    {
        public Type View { get; }
    }
}
