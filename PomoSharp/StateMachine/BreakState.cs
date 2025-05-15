namespace PomoSharp.StateMachine;

public abstract class BreakState(TimerStateMachine stateMachine) : TimerState(stateMachine)
{
    public override void OnExit()
    {
        if (!Settings.Data.ShouldAutoStartPomodoro)
        {
            StateMachine.CountdownTimer.Stop();
        }
    }

    public override void OnCompleted()
    {
        base.OnCompleted();

        StateMachine.TransitionTo(TimerStates.POMODORO);
    }

    protected override void UpdateReport()
    {
        base.UpdateReport();

        Report.Data.TotalBreakHours += Duration;
    }

    public override void OnEnter() { }
}