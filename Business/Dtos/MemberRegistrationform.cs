namespace Business.Dtos;

public class MemberRegistrationform
{
    public string? ImageUrl { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Title { get; set; } = null!;

    // Address
    public string Street { get; set; } = null!;
    public string ZipCode { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;


    // Birthdate
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}
