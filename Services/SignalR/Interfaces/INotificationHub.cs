namespace Services.SignalR.Interfaces;

public interface INotificationHub
{
    Task BroadcastMessage(string message);
}
