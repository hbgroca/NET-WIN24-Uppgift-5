namespace Business.Models;

public class AddressModel
{
    public int Id { get; set; }
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = "Sweden";
}
