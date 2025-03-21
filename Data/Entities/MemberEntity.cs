using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class MemberEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Title { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
    public DateOnly DateCreated { get; set; }
    public DateOnly DateUpdated { get; set; }

    public string Status { get; set; } = null!;

    // Adress (One-to-One)
    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = null!;

    // Projects (Many-to-Many)
    [JsonIgnore]
    public List<ProjectEntity> Projects { get; set; } = [];
}
