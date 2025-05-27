using System.Windows;

namespace PomoSharp;

public static class SingleInstance
{
    private static bool AlreadyProcessedOnThisInstance;

    internal static void Make(string appName, bool uniquePerUser = true)
    {
        if (AlreadyProcessedOnThisInstance)
        {
            return;
        }
        AlreadyProcessedOnThisInstance = true;

        Application app = Application.Current;

        string eventName = uniquePerUser
            ? $"{appName}-{Environment.MachineName}-{Environment.UserDomainName}-{Environment.UserName}"
            : $"{appName}-{Environment.MachineName}";

        bool isSecondaryInstance = true;

        EventWaitHandle? eventWaitHandle = null;
        
        try
        {
            eventWaitHandle = EventWaitHandle.OpenExisting(eventName);
        }
        catch
        {
            isSecondaryInstance = false;
        }

        if (isSecondaryInstance)
        {
            ActivateFirstInstanceWindow(eventWaitHandle!);
            Environment.Exit(0);
        }

        RegisterFirstInstanceWindowActivation(app, eventName);
    }

    private static void ActivateFirstInstanceWindow(EventWaitHandle eventWaitHandle)
    {
        _ = eventWaitHandle.Set();
    }

    private static void RegisterFirstInstanceWindowActivation(Application app, string eventName)
    {
        EventWaitHandle eventWaitHandle = new(
            false,
            EventResetMode.AutoReset,
            eventName);

        _ = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, WaitOrTimerCallback, app, Timeout.Infinite, false);

        eventWaitHandle.Close();
    }

    private static void WaitOrTimerCallback(object? state, bool timedOut)
    {
        if (state is Application app)
        {
            _ = app.Dispatcher.BeginInvoke(new Action(() =>
            {
                ShowCurrentMainWindow();
            }));
        }
    }

    public static void ShowCurrentMainWindow()
    {
        if (Application.Current.MainWindow != null)
        {
            Window window = Application.Current.MainWindow;

            if (!window.IsVisible)
            {
                window.Show();
            }

            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }

            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
        }
    }
}