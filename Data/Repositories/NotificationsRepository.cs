using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class NotificationsRepository(DataContext context, UserManager<MemberEntity> userManager) : BaseRepository<NotificationEntity>(context), INotificationsRepository
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 100)
    {
        var dismissedIds = await _context.NotificationDismisses
            .Where(x => x.UserId == userId)
            .Select(x => x.NotificationId)
            .ToListAsync();


        // Get the create time of user so we dont get notifications from before that date
        DateTime dateCreated = DateTime.UtcNow.AddDays(-7);
        if (userId != "Anonomous")
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)    
                dateCreated = DateTime.Parse(user!.DateCreated.ToString());
        }

        var notifications = await _context.Notifications
            .Where(u => u.Created >= dateCreated)
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
