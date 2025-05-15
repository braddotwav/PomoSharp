using PomoSharp.Models;

namespace PomoSharp.StateMachine;

public class ShortBreakState(TimerStateMachine stateMachine) : BreakState(stateMachine)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Short Break Finished",
        Message = "Let's get back to it. Another Pomodoro is ready."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Settings.Data.ShortDuration));
    }
}