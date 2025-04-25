using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class NotificationModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int TargetGroupId { get; set; } = 1; // Default to 1 (All)
        public NotificationTargetGroupEntity? TargetGroup { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationTypeEntity? NotificationType { get; set; } 
        public string? Image { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.Now;
        public ICollection<NotificationDismissEntity> Dismisses { get; set; } = [];
    }
}
