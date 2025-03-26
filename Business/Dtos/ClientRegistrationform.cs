namespace Business.Dtos;

public class ClientRegistrationform
{
    public string? ImageUrl { get; set; }

    public string ClientName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;


    // Address
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}
