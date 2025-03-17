using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
public class EditClientFormModel
{
    [DataType(DataType.Upload)]
    public IFormFile? ProfilePicture { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "Name", Prompt = "Enter name...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email address", Prompt = "Enter email address...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = " ")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phonenumber", Prompt = "Enter phonenumber...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^([0-9]{8,12})$", ErrorMessage = " ")]
    public string Phone { get; set; } = null!;

    // Address
    [Display(Name = "Streetname", Prompt = "Enter streetname...")]
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

    [Display(Name = "Status")]
    [Required(ErrorMessage = " ")]
    [MinLength(3, ErrorMessage = " ")]
    public string Status { get; set; } = null!;
}
