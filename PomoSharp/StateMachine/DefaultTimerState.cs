using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public class DefaultTimerState(TimerStateMachine stateMachine, IAppStorage storage) : TimerState(stateMachine, storage)
{
    public override void OnEnter()
    {
        StateMachine.TransitionTo(TimerStates.POMODORO);
    }

    public override void OnExit() { }
}