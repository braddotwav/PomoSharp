using PomoSharp.Models;
using PomoSharp.Services;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace PomoSharp.StateMachine;

public abstract class TimerState(TimerStateMachine stateMachine)
{
    public virtual NotificationContext CompletedNotificationContext { get; } = NotificationContext.DefaultTimerNotification;

    protected JsonStorageProvider<Settings> Settings = Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>();
    protected JsonStorageProvider<Report> Report = Ioc.Default.GetRequiredService<JsonStorageProvider<Report>>();
    protected TimerStateMachine StateMachine { get; private set; } = stateMachine;
    protected TimeSpan Duration => StateMachine.CountdownTimer.Duration;

    public virtual void OnCompleted()
    {
        UpdateReport();
        Report.Save();
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    protected virtual void UpdateReport()
    {
        Report.Data.TotalHours += Duration;
    }
}
