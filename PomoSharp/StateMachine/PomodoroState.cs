using PomoSharp.Models;
using PomoSharp.Services;

namespace PomoSharp.StateMachine;

public class PomodoroState(TimerStateMachine stateMachine, IAppStorage storage) : TimerState(stateMachine, storage)
{
    private int _pomodorosCompleted;

    public override NotificationContext CompletedNotificationContext => new()
    {
        Title = "Pomodoro Complete!",
        Message = "Nice work! Time for a break - you've earned it."
    };

    public override void OnEnter()
    {
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Storage.Settings.PomodoroDuration));
    }

    public override void OnExit()
    {
        if (!Storage.Settings.ShouldAutoStartBreak)
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
        Storage.Stats.PomodorosCompleted++;
        Storage.Stats.TotalFocusHours += Duration;
        Storage.Stats.DailyStreak = CalculateDailyStreak(Storage.Stats.LastPomodoroCompletedAt);
        Storage.Stats.LastPomodoroCompletedAt = DateTime.Now;
    }

    private bool IsLongBreakDue()
    {
        return _pomodorosCompleted % Storage.Settings.LongBreakInterval == 0;
    }

    private int CalculateDailyStreak(DateTime lastPomodoro)
    {
        var today = DateTime.Now.Date;
        var lastDate = lastPomodoro.Date;

        int dayDifference = (today - lastDate).Days;

        return dayDifference switch
        {
            0 => Storage.Stats.DailyStreak,
            1 => Storage.Stats.DailyStreak + 1,
            _ => 1
        };
    }
}
