using Business.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Business.Dtos;

public class AddProjectFormModel
{
    public IFormFile? ProjectImage { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter project name...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client")]
    public Guid ClientId { get; set; }


    [Display(Name = "Description", Prompt = "Type something...")]
    [Required(ErrorMessage = " ")]
    [MinLength(4, ErrorMessage = " ")]
    public string Description { get; set; } = null!;

    [Display(Name = "Start Date")]
    [Required(ErrorMessage = " ")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "End Date")]
    [Required(ErrorMessage = " ")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);


    [Display(Name = "Budget")]
    [Required(ErrorMessage = " ")]
    public decimal Budget { get; set; }


    // Json to store the members guid
    [Display(Name = "Members")]
    [Required(ErrorMessage = " ")]
    public string MembersJson { get; set; } = string.Empty;


    [JsonIgnore]
    public ClientModel? Client { get; set; }

    [JsonIgnore]
    public List<MemberModel> Members { get; set; } = [];
}
