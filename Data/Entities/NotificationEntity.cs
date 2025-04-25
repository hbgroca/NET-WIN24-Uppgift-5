using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class NotificationEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey(nameof(TargetGroup))]
    public int TargetGroupId { get; set; }
    [JsonIgnore]
    public NotificationTargetGroupEntity TargetGroup { get; set; } = null!;


    [ForeignKey(nameof(NotificationType))]
    public int NotificationTypeId { get; set; }
    [JsonIgnore]
    public NotificationTypeEntity NotificationType { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.Now;

    [JsonIgnore]
    public ICollection<NotificationDismissEntity> Dismisses { get; set; } = [];
}
