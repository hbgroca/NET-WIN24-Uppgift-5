using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Business.Dtos;

public class AddMemberFormModel
{
    public IFormFile? ProfilePicture { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "First Name", Prompt = "Enter first name...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter last name...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email address", Prompt = "Enter email address...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = " ")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phonenumber", Prompt = "Enter phonenumber...")]
    [Required(ErrorMessage = " ")]
    [RegularExpression(@"^([0-9]{8,12})$", ErrorMessage = " ")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Title", Prompt = "Enter members title...")]
    [Required(ErrorMessage = " ")]
    [MinLength(2, ErrorMessage = " ")]
    public string Title { get; set; } = null!;

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
    [MinLength(2, ErrorMessage =  " ")]
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
