using Data.Entities;

namespace Business.Interfaces;
public interface INotificationSerivces
{
    Task AddNotificationAsync(NotificationEntity entity, string userId = "anonymous");
    Task DismissAllNotificationsAsync(string userId);
    Task DismissNotificationAsync(string userId, string notificationId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 10);
}