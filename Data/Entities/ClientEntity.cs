﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data.Entities;

public class ClientEntity
{
    [Key]
    public Guid Id { get; set; }

    public string? ImageUrl { get; set; }

    public string ClientName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public DateOnly DateCreated { get; set; }
    public DateOnly DateUpdated { get; set; }

    public string Status { get; set; } = null!;


    // Address (One-to-One)
    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = null!;

    // Projects (One-to-Many)
    [JsonIgnore]
    public List<ProjectEntity> Projects { get; set; } = [];
}
