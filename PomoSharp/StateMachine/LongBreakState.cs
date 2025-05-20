using PomoSharp.Models;
using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public class LongBreakState(TimerStateMachine stateMachine, IAppStorage storage) : BreakState(stateMachine, storage)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Long Break Finished",
        Message = "You've refreshed and recharged - time to refocus."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Storage.Settings.LongDuration));
    }
}