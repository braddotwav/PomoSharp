using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public enum TimerStates
{
    POMODORO = 0,
    SHORT_BREAK = 1,
    LONG_BREAK = 2
}

public class TimerStateMachine
{
    public CountdownTimer CountdownTimer { get; private set; }
    public event Action<TimerStates>? OnStateChanged;
    public TimerState CurrentState;

    private readonly List<TimerState> _states;
    private readonly IAppStorage _storage;

    public TimerStateMachine(CountdownTimer countdownTimer, IAppStorage storage)
    {
        _storage = storage;
        _states = [new PomodoroState(this, _storage), new ShortBreakState(this, _storage), new LongBreakState(this, _storage)];

        CountdownTimer = countdownTimer;
        CountdownTimer.OnComplete += OnCountdownComplete;

        CurrentState = new DefaultTimerState(this, _storage);
        CurrentState.OnEnter();
    }

    private void OnCountdownComplete()
    {
        CurrentState?.OnCompleted();
    }

    public void TransitionTo(TimerStates state)
    {
        CurrentState?.OnExit();
        CurrentState = _states[(int)state];
        CurrentState?.OnEnter();
        OnStateChanged?.Invoke(state);
    }

    public void RefreshCurrentState()
    {
        CurrentState?.OnEnter();
    }
}