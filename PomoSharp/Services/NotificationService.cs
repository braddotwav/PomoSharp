using PomoSharp.Models;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PomoSharp.Services;

public class NotificationService : INotificationService
{
    public void Push(NotificationContext context)
    {
        var notification = new ToastContentBuilder()
            .AddText(context.Title)
            .AddText(context.Message);

        notification.Show(toast => 
        {
            toast.ExpirationTime = DateTime.Now.AddSeconds(5);
        });
    }
}