using PomoSharp.Models;

namespace PomoSharp.StateMachine;

public class LongBreakState(TimerStateMachine stateMachine) : BreakState(stateMachine)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Long Break Finished",
        Message = "You've refreshed and recharged - time to refocus."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Settings.Data.LongDuration));
    }
}