using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Data.Repositories;

public class NotificationsRepository(DataContext context) : BaseRepository<NotificationEntity>(context), INotificationsRepository
{
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 5)
    {
        var dismissedIds = await _context.NotificationDismisses
            .Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = await _context.Notifications
            .Where(x => !dismissedIds.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take)
            .ToListAsync();

        return notifications ?? [];
    }
}
