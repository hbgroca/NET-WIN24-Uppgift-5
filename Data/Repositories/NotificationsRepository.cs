using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class NotificationsRepository(DataContext context) : BaseRepository<NotificationEntity>(context), INotificationsRepository
{
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 100)
    {
        var dismissedIds = await _context.NotificationDismisses
            .Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .Include(x => x.NotificationType)
            .Include(x => x.TargetGroup)
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();

        return notifications ?? [];
    }

    public async Task<bool> DismissNotificationAsync(string userId, string notificationId)
    {
        var alreadyDismissed = await _context.NotificationDismisses
            .AnyAsync(x => x.UserId == userId && x.NotificationId == notificationId);

        return alreadyDismissed;
    }

    public async Task<bool> AddDismissNotificationAsync(NotificationDismissEntity entity)
    {
        await _context.NotificationDismisses.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
