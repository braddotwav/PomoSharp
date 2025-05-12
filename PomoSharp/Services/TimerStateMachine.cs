using PomoSharp.Models;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace PomoSharp.Services;

public class TimerStateMachine
{
    public CountdownTimer CountdownTimer { get; private set; }
   
    public event Action<TimerStates>? OnStateChanged;

    public TimerState CurrentState;

    private readonly List<TimerState> _states;

    public TimerStateMachine(CountdownTimer countdownTimer)
    {
        _states = [new PomodoroState(this), new ShortBreakState(this), new LongBreakState(this)];
        
        CountdownTimer = countdownTimer;
        CountdownTimer.OnComplete += OnCountdownComplete;

        CurrentState = new InitialiseState(this);
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
}

public enum TimerStates
{
    POMODORO = 0,
    SHORT_BREAK = 1,
    LONG_BREAK = 2
}

public abstract class TimerState(TimerStateMachine stateMachine)
{
    public virtual NotificationContext CompletedNotificationContext { get; } = NotificationContext.DefaultTimerNotification;

    protected JsonStorageProvider<Settings> Settings = Ioc.Default.GetRequiredService<JsonStorageProvider<Settings>>();
    protected JsonStorageProvider<Report> Report = Ioc.Default.GetRequiredService<JsonStorageProvider<Report>>();
    protected TimerStateMachine StateMachine { get; private set; } = stateMachine;
    protected TimeSpan Duration => StateMachine.CountdownTimer.Duration;

    public virtual void OnCompleted()
    {
        UpdateReport();
    }

    public abstract void OnEnter();

    public abstract void OnExit();
    
    protected virtual void UpdateReport()
    {
        Report.Data.TotalHours += Duration;
    }
}

public class InitialiseState(TimerStateMachine stateMachine) : TimerState(stateMachine)
{
    public override void OnEnter()
    {
        StateMachine.TransitionTo(TimerStates.POMODORO);
    }

    public override void OnExit() { }
}

public class PomodoroState(TimerStateMachine stateMachine) : TimerState(stateMachine)
{
    private int _pomodorosCompleted;

    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Pomodoro Complete!",
        Message = "Nice work! Time for a break - you've earned it."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(1));
    }

    public override void OnExit()
    {
        if (!Settings.Data.ShouldAutoStartBreak) 
        {
            StateMachine.CountdownTimer.Stop();
        }
    }

    public override void OnCompleted()
    {
        base.OnCompleted();

        _pomodorosCompleted++;
        StateMachine.TransitionTo(IsLongBreakDue() ? TimerStates.LONG_BREAK : TimerStates.SHORT_BREAK);
    }

    protected override void UpdateReport()
    {
        base.UpdateReport();
        Report.Data.PomodorosCompleted++;
        Report.Data.TotalFocusHours += Duration;
        Report.Data.DailyStreak = CalculateDailyStreak(Report.Data.LastPomodoroCompletedAt);
        Report.Data.LastPomodoroCompletedAt = DateTime.Now;
    }

    private bool IsLongBreakDue()
    {
        return _pomodorosCompleted % Settings.Data.LongBreakInterval == 0;
    }

    private int CalculateDailyStreak(DateTime lastPomodoro)
    {
        var today = DateTime.Now.Date;
        var lastDate = lastPomodoro.Date;

        int dayDifference = (today - lastDate).Days;

        return dayDifference switch
        {
            0 => Report.Data.DailyStreak,
            1 => Report.Data.DailyStreak + 1,
            _ => 1
        };
    }
}

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

public class ShortBreakState(TimerStateMachine stateMachine) : BreakState(stateMachine)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Short Break Finished",
        Message = "Let's get back to it. Another Pomodoro is ready."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Settings.Data.TimerDurations.Short));
    }
}

public class LongBreakState(TimerStateMachine stateMachine) : BreakState(stateMachine)
{
    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Long Break Finished",
        Message = "You've refreshed and recharged - time to refocus."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Settings.Data.TimerDurations.Long));
    }
}