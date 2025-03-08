using System.ComponentModel.DataAnnotations;

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
    
    public decimal Budget { get; set; }


    // Client (One-to-Many)
    public Guid ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;


    // Members (Many-to-Many)
    public List<MemberEntity> Members { get; set; } = [];
}
