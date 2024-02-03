#nullable enable

using System;
using Windows.UI.Xaml;

namespace pwm.Core.API
{
    public interface IApplicationEventDispatcher
    {
        event EventHandler<object> Resuming;

        event SuspendingEventHandler Suspending;

        event EnteredBackgroundEventHandler EnteredBackground;

        event LeavingBackgroundEventHandler LeavingBackground;

        event Windows.UI.Xaml.UnhandledExceptionEventHandler UnhandledException;
    }
}
