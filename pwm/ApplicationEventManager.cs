#nullable enable

using pwm.Core.API;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;

namespace pwm
{
    public class ApplicationEventManager : IApplicationEventManager
    {
        private readonly ILogger logger;

        private readonly List<Action<object, object>> resuming = new();

        private readonly List<Action<object, SuspendingEventArgs?>> suspending = new();

        private readonly List<Action<object, EnteredBackgroundEventArgs>> enteredBackground = new();

        private readonly List<Action<object, LeavingBackgroundEventArgs>> leavingBackground = new();

        private readonly List<Action<object, Windows.UI.Xaml.UnhandledExceptionEventArgs>> unhandledException = new();

        private readonly List<IClosingHandler> closingHandlers = new();

        public ApplicationEventManager(ILogger logger) : this(logger, new DefaultApplicationEventDispatcher()) { }

        public ApplicationEventManager(ILogger logger, IApplicationEventDispatcher dispatcher)
        {
            this.logger = logger;
            dispatcher.Resuming += Resuming;
            dispatcher.Suspending += Suspending;
            dispatcher.EnteredBackground += EnteredBackground;
            dispatcher.LeavingBackground += LeavingBackground;
            dispatcher.UnhandledException += UnhandledException;

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += ApplicationEventManager_CloseRequested;
        }

        public Action AddClosingHandler(IClosingHandler handler)
        {
            closingHandlers.Add(handler);
            return () => { _ = closingHandlers.Remove(handler); };
        }

        public void Exit()
        {
            Suspending(this, null);
            App.Current.Exit();
        }

        private void Resuming(object sender, object e)
        {
            logger.Info("OnResuming.");
            resuming.ForEach(x =>
            {
                SafeInvoke(() => x.Invoke(sender, e));
            });
        }

        private void Suspending(object sender, SuspendingEventArgs? e)
        {
            logger.Info("OnSuspending.");
            suspending.ForEach(x =>
            {
                SafeInvoke(() => x.Invoke(sender, e));
            });
        }

        private void EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            logger.Info("OnEnteredBackground.");
            enteredBackground.ForEach(x =>
            {
                SafeInvoke(() => x.Invoke(sender, e));
            });
        }
        private void LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            logger.Info("OnLeavingBackground.");
            leavingBackground.ForEach(x =>
            {
                SafeInvoke(() => x.Invoke(sender, e));
            });
        }

        private void UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            logger.Info("OnUnhandledException.");
            unhandledException.ForEach(x =>
            {
                SafeInvoke(() => x.Invoke(sender, e));
            });
        }

        private void ApplicationEventManager_CloseRequested(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            closingHandlers.ForEach(h =>
            {
                if (h.Handled())
                {
                    e.Handled = true;
                }
            });
        }

        public Action RegisterResuming(Action<object, object> handler)
        {
            resuming.Add(handler);
            return () => resuming.Remove(handler);
        }

        public Action RegisterSuspending(Action<object, SuspendingEventArgs?> handler)
        {
            suspending.Add(handler);
            return () => suspending.Remove(handler);
        }

        public Action RegisterEnteredBackground(Action<object, EnteredBackgroundEventArgs> handler)
        {
            enteredBackground.Add(handler);
            return () => enteredBackground.Remove(handler);
        }

        public Action RegisterLeavingBackground(Action<object, LeavingBackgroundEventArgs> handler)
        {
            leavingBackground.Add(handler);
            return () => leavingBackground.Remove(handler);
        }

        public Action RegisterUnhandledException(Action<object, Windows.UI.Xaml.UnhandledExceptionEventArgs> handler)
        {
            unhandledException.Add(handler);
            return () => unhandledException.Remove(handler);
        }

        private void SafeInvoke(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
                logger.Warn(ex.StackTrace);
            }
        }

        private class DefaultApplicationEventDispatcher : IApplicationEventDispatcher
        {
            public event EventHandler<object>? Resuming;
            public event SuspendingEventHandler? Suspending;
            public event EnteredBackgroundEventHandler? EnteredBackground;
            public event LeavingBackgroundEventHandler? LeavingBackground;
            public event Windows.UI.Xaml.UnhandledExceptionEventHandler? UnhandledException;

            public DefaultApplicationEventDispatcher()
            {
                App.Current.Resuming += Current_Resuming;
                App.Current.Suspending += Current_Suspending;
                App.Current.EnteredBackground += Current_EnteredBackground;
                App.Current.LeavingBackground += Current_LeavingBackground;
                App.Current.UnhandledException += Current_UnhandledException;
            }

            private void Current_Resuming(object sender, object e)
            {
                Resuming?.Invoke(sender, e);
            }

            private void Current_Suspending(object sender, SuspendingEventArgs e)
            {
                Suspending?.Invoke(sender, e);
            }

            private void Current_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
            {
                EnteredBackground?.Invoke(sender, e);
            }

            private void Current_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
            {
                LeavingBackground?.Invoke(sender, e);
            }

            private void Current_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
            {
                UnhandledException?.Invoke(sender, e);
            }
        }
    }
}





