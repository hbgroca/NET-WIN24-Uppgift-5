using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Business.Hubs;

public class NotificationHub(UserManager<MemberEntity> userManager) : Hub
{
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public override async Task OnConnectedAsync()
    {
        var user = await _userManager.GetUserAsync(Context.User);
        if (user != null)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }
        }
        await base.OnConnectedAsync();
    }

    public async Task SendNotification(NotificationModel notification)
    {
        await Clients.All.SendAsync("AllReceiveNotification", notification);
    }

    public async Task SendNotificationToAdmins(NotificationModel notification)
    {
        await Clients.Group("Admin").SendAsync("AdminReceiveNotification", notification);
    }
}
