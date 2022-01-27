using Microsoft.AspNetCore.SignalR;
using Services.SignalR.Interfaces;

namespace Services.SignalR.Hubs;

public class NotificationHub : Hub<INotificationHub>
{
    public NotificationHub()
    {
    }
}
