using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class ClientStatusFormModel
{
    [Display(Name = "Status", Prompt = "New status...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string Description { get; set; } = null!;
}
