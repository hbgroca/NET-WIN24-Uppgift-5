using Business.Hubs;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Business.Services;

public class NotificationSerivces(INotificationsRepository notificationsRepository, IHubContext<NotificationHub> hubContext) : INotificationSerivces
{
    private readonly INotificationsRepository _notificationsRepository = notificationsRepository;
    private readonly IHubContext<NotificationHub> _notificationHub = hubContext;

    public async Task AddNotificationAsync(NotificationEntity entity, string userId = "anonymous")
    {
        // If there is no image, set a default image based on the notification type
        if (string.IsNullOrEmpty(entity.Image))
        {
            switch (entity.NotificationTypeId)
            {
                case 1: // Client
                    {
                        entity.Image = "/images/defaultmember.png";
                        break;
                    }
                case 2: // Project
                    {
                        entity.Image = "/images/defaultproject.png";
                        break;
                    }
                case 3: // Member
                    {
                        entity.Image = "/images/defaultmember.png";
                        break;
                    }
            }
        }

        // Save the notification to the database
        await _notificationsRepository.BeginTransactionAsync();
        try
        {
            var result = await _notificationsRepository.CreateAsync(entity);
            if (!result)
                Debug.WriteLine("Failed to create notification");

            await _notificationsRepository.SaveAsync();
            await _notificationsRepository.CommitTransactionAsync();

            // Send notification to the users
            switch (entity.TargetGroupId)
            {
                case 1:
                    {
                        // All users
                        await _notificationHub.Clients.All.SendAsync("AllReceiveNotification", entity);
                        break;
                    }
                case 2:
                    {
                        // Admins only
                        await _notificationHub.Clients.Group("Admin").SendAsync("AdminReceiveNotification", entity);
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating notification: {ex.Message}");
            await _notificationsRepository.RollbackTransactionAsync();
        }
    }



    // Get notifications from db
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 100)
    {
        var notifications = await _notificationsRepository.GetNotificationsAsync(userId, take);

        return notifications;
    }



    // Dismiss notification for current user
    public async Task DismissNotificationAsync(string userId, string notificationId)
    {
        var alreadyDismissed = await _notificationsRepository.DismissNotificationAsync(userId, notificationId);

        if (alreadyDismissed)
            return;

        var notificationDismiss = new NotificationDismissEntity
        {
            UserId = userId,
            NotificationId = notificationId
        };

        var result = await _notificationsRepository.AddDismissNotificationAsync(notificationDismiss);
        if (!result)
            Debug.WriteLine("Failed to dismiss notification");
    }

    // Dismiss all notifications for current user
    public async Task DismissAllNotificationsAsync(string userId)
    {

        var notifications = await _notificationsRepository.GetNotificationsAsync(userId, 9999);

        foreach (var notification in notifications)
        {
            var notificationDismiss = new NotificationDismissEntity
            {
                UserId = userId,
                NotificationId = notification.Id
            };

            await _notificationsRepository.AddDismissNotificationAsync(notificationDismiss);
        }
    }
}
