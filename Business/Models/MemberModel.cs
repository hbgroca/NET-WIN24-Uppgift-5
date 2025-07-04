﻿namespace Business.Models;

public class MemberModel
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Title { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }

    public AddressModel Address { get; set; } = null!;
    public List<ProjectModel> Projects { get; set; } = [];

    public string Status { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";
}
