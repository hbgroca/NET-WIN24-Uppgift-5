using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class MemberLoginFormModel
{
    [Display(Name = "Email Address",Prompt = "Enter email adress...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = " ")]
    public string Email { get; set; } = null!;


    [Display(Name = "Password", Prompt = "Your secret password...")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Invalid")]
    public string Password { get; set; } = null!;
}
