using Data.Entities;

namespace Data.Interfaces;

public interface INotificationsRepository : IBaseRepository<NotificationEntity>
{
    Task<bool> AddDismissNotificationAsync(NotificationDismissEntity entity);
    Task<bool> DismissNotificationAsync(string userId, string notificationId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync( string userId, int take = 5);
}