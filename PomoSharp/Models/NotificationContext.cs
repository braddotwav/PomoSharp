namespace PomoSharp.Models;

public class NotificationContext
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public static readonly NotificationContext DefaultTimerNotification = new NotificationContext()
    {
        Title = "Timer Completed",
        Message = "A timer has completed."
    };
}