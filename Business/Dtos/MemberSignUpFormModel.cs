using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class MemberSignUpFormModel
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "Invalid")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Invalid")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Invalid")]
    [EmailAddress(ErrorMessage = "Invalid")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    [MinLength(8, ErrorMessage = "Min 8 chars")]
    [Required(ErrorMessage = "Invalid")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Invalid")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Not matching")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms and Conditions")]
    [Required(ErrorMessage = " ")]
    public bool TermsAndConditions { get; set; }


    [Display(Name = "Phone Number", Prompt = "Enter phone number...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^([0-9]{8,12})$", ErrorMessage = "Invalid")]
    public string PhoneNumber { get; set; } = null!;

    // Address
    [Display(Name = "Street Name", Prompt = "Enter street name...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string Street { get; set; } = null!;

    [Display(Name = "Zip Code", Prompt = "Enter zip code...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^[0-9]{4,6}$", ErrorMessage = "Invalid")]
    public string ZipCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter city...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string City { get; set; } = null!;

    [Display(Name = "Country", Prompt = "Enter country...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string Country { get; set; } = null!;


    // Birthdate
    [Display(Name = "Year", Prompt = "Enter year...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Invalid")]
    public int Year { get; set; }

    [Display(Name = "Month", Prompt = "Enter month...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Invalid")]
    public int Month { get; set; }

    [Display(Name = "Day", Prompt = "Enter day...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^(0?[1-9]|[31][0-9]|3[01])$", ErrorMessage = "Invalid")]
    public int Day { get; set; }
}
