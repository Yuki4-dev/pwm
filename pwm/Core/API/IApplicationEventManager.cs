#nullable enable

using System;
using Windows.ApplicationModel;

namespace pwm.Core.API
{
    public interface IApplicationEventManager
    {
        Action AddClosingHandler(IClosingHandler handler);

        void Exit();

        Action RegisterEnteredBackground(Action<object, EnteredBackgroundEventArgs> handler);

        Action RegisterLeavingBackground(Action<object, LeavingBackgroundEventArgs> handler);

        Action RegisterResuming(Action<object, object> handler);

        Action RegisterSuspending(Action<object, SuspendingEventArgs?> handler);

        Action RegisterUnhandledException(Action<object, Windows.UI.Xaml.UnhandledExceptionEventArgs> handler);
    }
}
