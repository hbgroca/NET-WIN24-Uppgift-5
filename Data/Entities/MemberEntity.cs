using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class MemberEntity : IdentityUser
{
    public string? ImageUrl { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public string Status { get; set; } = null!;

    public int AddressId { get; set; }
    public AddressEntity? Address { get; set; } = null!;

    [JsonIgnore]
    public List<ProjectEntity> Projects { get; set; } = [];

    public ICollection<NotificationDismissEntity> DismissedNotifications { get; set; } = [];
}
