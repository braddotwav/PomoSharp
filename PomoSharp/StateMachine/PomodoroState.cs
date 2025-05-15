using PomoSharp.Models;

namespace PomoSharp.StateMachine;

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
        StateMachine.CountdownTimer.SetDuration(TimeSpan.FromMinutes(Settings.Data.PomodoroDuration));
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