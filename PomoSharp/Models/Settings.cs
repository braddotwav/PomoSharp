namespace PomoSharp.Models;

public class Settings : IEquatable<Settings>
{
    public int PomodoroDuration { get; set; } = 25;
    public int ShortDuration { get; set; } = 5;
    public int LongDuration { get; set; } = 15;
    public int LongBreakInterval { get; set; } = 4;
    public bool ShouldAutoStartBreak { get; set; } = false;
    public bool ShouldAutoStartPomodoro { get; set; } = false;
    public bool AllowNotifications { get; set; } = true;
    public WindowBounds? WindowBounds { get; set; }

    public Settings() { }

    public Settings(Settings settings)
    {
        PomodoroDuration = settings.PomodoroDuration;
        ShortDuration = settings.ShortDuration;
        LongDuration = settings.LongDuration;
        LongBreakInterval = settings.LongBreakInterval;
        ShouldAutoStartBreak = settings.ShouldAutoStartBreak;
        ShouldAutoStartPomodoro = settings.ShouldAutoStartPomodoro;
        AllowNotifications = settings.AllowNotifications;
        WindowBounds = settings.WindowBounds;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Settings);
    }

    public bool Equals(Settings? other)
    {
        if (other is null) return false;

        return PomodoroDuration == other.PomodoroDuration &&
               ShortDuration == other.ShortDuration &&
               LongDuration == other.LongDuration &&
               LongBreakInterval == other.LongBreakInterval &&
               ShouldAutoStartBreak == other.ShouldAutoStartBreak &&
               ShouldAutoStartPomodoro == other.ShouldAutoStartPomodoro &&
               AllowNotifications == other.AllowNotifications;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            PomodoroDuration,
            ShortDuration,
            LongDuration,
            LongBreakInterval,
            ShouldAutoStartBreak,
            ShouldAutoStartPomodoro,
            AllowNotifications
        );
    }
}