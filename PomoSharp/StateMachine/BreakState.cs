using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public abstract class BreakState(TimerStateMachine stateMachine, IAppStorage storage) : TimerState(stateMachine, storage)
{
    public override void OnExit()
    {
        if (!Storage.Settings.ShouldAutoStartPomodoro)
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

        Storage.Stats.TotalBreakHours += Duration;
    }

    public override void OnEnter() { }
}