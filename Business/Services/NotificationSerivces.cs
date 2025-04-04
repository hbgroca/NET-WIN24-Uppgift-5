using Business.Hubs;
using Business.Interfaces;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Business.Services;

public class NotificationSerivces(IHubContext<NotificationHub> notificationhub, INotificationsRepository notificationsRepository) : INotificationSerivces
{
    private readonly INotificationsRepository _notificationsRepository = notificationsRepository;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationhub;


    public async Task AddNotificationAsync(int notificationTypeId, string message, string userId = "anonymous", string image = null!, int notificationTargetGroup = 1)
    {
        if (string.IsNullOrEmpty(image))
        {
            switch (notificationTypeId)
            {
                // Client
                case 1:
                    {
                        image = "/images/defaultmember.png";
                        break;
                    }
                // Project
                case 2:
                    {
                        image = "/images/defaultmember.png";
                        break;
                    }
                // Member
                case 3:
                    {
                        image = "/images/defaultmember.png";
                        break;
                    }
            }
        }

        var notificationEntity = new NotificationEntity
        {
            TargetGroupId = notificationTargetGroup,
            NotificationTypeId = notificationTypeId,
            Message = message,
            Image = image
        };

        // Save the notification to the database
        await _notificationsRepository.BeginTransactionAsync();
        try
        {
            var result = await _notificationsRepository.CreateAsync(notificationEntity);
            if (!result)
                Debug.WriteLine("Failed to create notification");

            await _notificationsRepository.SaveAsync();
            await _notificationsRepository.CommitTransactionAsync();

            // Send notification to the user
            var notifications = await GetNotificationsAsync(userId);
            var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

            // Send notification to hub 
            var notificationObject = new
            {
                id = newNotification?.Id,
                message = newNotification?.Message,
                image = newNotification?.Image,
                created = newNotification?.Created,
            };

            switch (notificationTargetGroup)
            {
                case 1:
                    {
                        // All users
                        await _notificationHub.Clients.All.SendAsync("AllReceiveNotification", notificationObject);
                        break;
                    }
                case 2:
                    {
                        // Admins only
                        await _notificationHub.Clients.Group("Admins").SendAsync("AdminReceiveNotification", notificationObject);
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
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10)
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

        await _notificationHub.Clients.User(userId).SendAsync("DismissNotification", notificationId);
    }
}
