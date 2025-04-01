using Data.Entities;

namespace Data.Interfaces;

public interface INotificationsRepository : IBaseRepository<NotificationEntity>
{
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 5);
}