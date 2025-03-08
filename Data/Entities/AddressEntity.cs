using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = "Sweden";
}
