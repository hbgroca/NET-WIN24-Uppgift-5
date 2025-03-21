using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly CreateDate { get; set; }
    public DateOnly UpdateDate { get; set; }

    public decimal Budget { get; set; }
    public bool IsCompleted { get; set; }


    // Client (One-to-Many)
    public Guid ClientId { get; set; }
    [JsonIgnore]
    public ClientEntity Client { get; set; } = null!;


    // Members (Many-to-Many)
    [JsonIgnore]
    public List<MemberEntity> Members { get; set; } = [];
}
