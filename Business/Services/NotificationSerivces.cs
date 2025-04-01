using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class NotificationSerivces(INotificationsRepository notificationsRepository)
{
    private readonly INotificationsRepository _notificationsRepository = notificationsRepository;

    public async Task AddNotificationAsync(int notificationTypeId, string message, string image = null!, int notificationTargetGroup = 1)
    {
        if (string.IsNullOrEmpty(image))
        {
            switch (notificationTypeId)
            {
                case 1:
                    {
                        image = "/images/defaultmember.png";
                        break;
                    }
                case 2:
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

        await _notificationsRepository.CreateAsync(notificationEntity);
    }


    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string userId, int take = 5)
    {
        var notifications = await _notificationsRepository.GetNotificationsAsync(userId, take);


        return []; 
    }
}
