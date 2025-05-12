using PomoSharp.Models;

namespace PomoSharp.Services;

public interface INotificationService
{
    public void Push(NotificationContext cxt);
}