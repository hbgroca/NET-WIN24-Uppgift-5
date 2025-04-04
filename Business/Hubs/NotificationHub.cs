using Microsoft.AspNetCore.SignalR;

namespace Business.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(object notification)
    {
        await Clients.All.SendAsync("AllReceiveNotification", notification);
    }

    public async Task SendNotificationToAdmins(object notification)
    {
        await Clients.Group("Admins").SendAsync("AdminReceiveNotification", notification);
    }
}
