using PomoSharp.Models;
using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public abstract class TimerState(TimerStateMachine stateMachine, IAppStorage storage)
{
    public virtual NotificationContext CompletedNotificationContext { get; } = NotificationContext.DefaultTimerNotification;

    protected IAppStorage Storage = storage;
    protected TimerStateMachine StateMachine { get; private set; } = stateMachine;
    protected TimeSpan Duration => StateMachine.CountdownTimer.Duration;

    public virtual void OnCompleted()
    {
        UpdateReport();
        Storage.Save();
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    protected virtual void UpdateReport()
    {
        Storage.Stats.TotalHours += Duration;
    }
}
