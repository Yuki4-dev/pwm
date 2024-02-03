#nullable enable

using System;

namespace pwm.Core.Models
{
    public class NavigationPageMessage
    {
        public Type PageType { get; }

        public object? Parameter { get; }

        public NavigationPageMessage(Type pageType, object? parameter = null)
        {
            PageType = pageType;
            Parameter = parameter;
        }
    }
}
