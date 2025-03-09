namespace Business.Models;

public class ClientModel
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }

    public string ClientName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public DateOnly DateCreated { get; set; }
    public DateOnly DateUpdated { get; set; }

    public AddressModel Address { get; set; } = null!;

    public List<ProjectModel> Projects { get; set; } = [];
}
