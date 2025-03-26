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
}
