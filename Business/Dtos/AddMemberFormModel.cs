using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Business.Dtos;

public class AddMemberFormModel
{
    public IFormFile? ProfilePicture { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "First Name", Prompt = "Enter first name...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter last name...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email address", Prompt = "Enter email address...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phonenumber", Prompt = "Enter phonenumber...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^([0-9]{8,12})$", ErrorMessage = "Invalid")]
    public string Phone { get; set; } = null!;

    [Display(Name = "Title", Prompt = "Enter members title...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string Title { get; set; } = null!;

    // Address
    [Display(Name = "Streetname", Prompt = "Enter streetname...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string Street { get; set; } = null!;

    [Display(Name = "Zip Code", Prompt = "Enter zip code...")]
    [Required(ErrorMessage = "Invalid")]
    [RegularExpression(@"^[0-9]{4,6}$", ErrorMessage = "Invalid")]
    public string ZipCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter city...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage =  "Invalid")]
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
