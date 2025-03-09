namespace Business.Dtos;

public class AddressRegistrationform
{
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = "Sweden";
}
