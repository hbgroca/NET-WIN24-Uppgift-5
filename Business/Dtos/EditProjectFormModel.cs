using Business.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Business.Dtos;

public class EditProjectFormModel
{
    public Guid Id { get; set; }
    public IFormFile? ProjectImage { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter project name...")]
    [Required(ErrorMessage = "Invalid")]
    [MinLength(2, ErrorMessage = "Invalid")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client")]
    [Required(ErrorMessage = "Invalid")]
    public Guid ClientId { get; set; }


    [Display(Name = "Description", Prompt = "Type something...")]
    [Required(ErrorMessage = "Invalid")]
    public string? Description { get; set; }

    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Invalid")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Display(Name = "End Date")]
    [Required(ErrorMessage = "Invalid")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);


    [Display(Name = "Budget")]
    [Required(ErrorMessage = "Invalid)")]
    public decimal Budget { get; set; }


    [Display(Name = "Status")]
    public bool IsCompleted { get; set; }


    // Json to store the members guid
    [Display(Name = "Members")]
    [Required(ErrorMessage = "Invalid")]
    public string MembersJson { get; set; } = string.Empty;


    [JsonIgnore]
    public ClientModel? Client { get; set; }

    [JsonIgnore]
    public List<MemberModel> Members { get; set; } = [];
}
