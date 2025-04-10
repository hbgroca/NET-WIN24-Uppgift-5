using Data.Entities;

namespace Business.Interfaces;
public interface INotificationSerivces
{
    Task AddNotificationAsync(int notificationTypeId, string message, string userId = "anonymous", string image = null!, int notificationTargetGroup = 1);
    Task DismissAllNotificationsAsync(string userId);
    Task DismissNotificationAsync(string userId, string notificationId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
}