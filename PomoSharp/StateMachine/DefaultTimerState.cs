namespace PomoSharp.StateMachine;

public class DefaultTimerState(TimerStateMachine stateMachine) : TimerState(stateMachine)
{
    public override void OnEnter()
    {
        StateMachine.TransitionTo(TimerStates.POMODORO);
    }

    public override void OnExit() { }
}