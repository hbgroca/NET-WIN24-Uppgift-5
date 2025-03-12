using Business.Models;
using Data.Entities;

namespace Business.Dtos;

public class ProjectRegistrationform
{
    public string? ImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public decimal Budget { get; set; }

    public ClientModel Client { get; set; } = null!;

    public List<MemberModel> Members { get; set; } = [];
}
