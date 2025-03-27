using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberSignUpFormModel
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = " ")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = " ")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = " ")]
    [EmailAddress(ErrorMessage = " ")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    [MinLength(8, ErrorMessage = " ")]
    [Required(ErrorMessage = " ")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = " ")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = " ")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms and Conditions")]
    [Required(ErrorMessage = "Accept our terms and conditions to continue")]
    public bool TermsAndConditions { get; set; }


    [Display(Name = "Phone Number", Prompt = "Enter phone number...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^([0-9]{8,12})$", ErrorMessage = " ")]
    public string PhoneNumber { get; set; } = null!;

    // Address
    [Display(Name = "Street Name", Prompt = "Enter street name...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string Street { get; set; } = null!;

    [Display(Name = "Zip Code", Prompt = "Enter zip code...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^[0-9]{4,6}$", ErrorMessage = " ")]
    public string ZipCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter city...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string City { get; set; } = null!;

    [Display(Name = "Country", Prompt = "Enter country...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string Country { get; set; } = null!;


    // Birthdate
    [Display(Name = "Year", Prompt = "Enter year...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^[0-9]{4}$", ErrorMessage = " ")]
    public int Year { get; set; }

    [Display(Name = "Month", Prompt = "Enter month...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = " ")]
    public int Month { get; set; }

    [Display(Name = "Day", Prompt = "Enter day...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^(0?[1-9]|[31][0-9]|3[01])$", ErrorMessage = " ")]
    public int Day { get; set; }
}
