namespace PomoSharp.Models;

public class Settings
{
    public TimerDurations TimerDurations { get; set; } = new();
    public int LongBreakInterval { get; set; } = 4;
    public bool ShouldAutoStartBreak { get; set; } = false;
    public bool ShouldAutoStartPomodoro { get; set; } = false;
    public bool AllowNotifications { get; set; } = true;
    public Theme Theme { get; set; } = Theme.LIGHT;
    public WindowBounds? WindowBounds { get; set; }
}

public enum Theme
{
    LIGHT = 0,
    DARK = 1,
}