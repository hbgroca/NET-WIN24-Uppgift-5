using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class MemberLoginFormModel
{
    [Display(Name = "Email Address",Prompt = "Enter email adress...")]
    [Required(ErrorMessage = " ")]
    public string? Email { get; set; }


    [Display(Name = "Password", Prompt = "Your secret password...")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = " ")]
    public string? Password { get; set; }
}
