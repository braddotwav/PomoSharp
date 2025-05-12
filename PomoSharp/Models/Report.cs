namespace PomoSharp.Models;

public class Report
{
    public TimeSpan TotalHours { get; set; }
    public TimeSpan TotalFocusHours { get; set; }
    public TimeSpan TotalBreakHours { get; set; }
    public DateTime LastPomodoroCompletedAt { get; set; }
    public int PomodorosCompleted { get; set; }
    public int DailyStreak { get; set; }
}