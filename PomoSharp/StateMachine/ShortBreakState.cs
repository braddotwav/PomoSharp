using PomoSharp.Models;
using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public class ShortBreakState(TimerStateMachine stateMachine, IAppStorage storage) : BreakState(stateMachine, storage)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Short Break Finished",
        Message = "Let's get back to it. Another Pomodoro is ready."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Storage.Settings.ShortDuration));
    }
}
