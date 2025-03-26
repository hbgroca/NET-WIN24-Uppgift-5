using Business.Models;
using Data.Entities;
using System.Diagnostics.Metrics;
using System.IO;
using System.Numerics;
using System.Reflection.Emit;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity Create(AddMemberFormModel form)
    {
        return new MemberEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            ImageUrl = form.ImageName,
            PhoneNumber = form.Phone,
            Title = form.Title,
            Status = "Active",
            BirthDate = DateOnly.Parse($"{form.Day}-{form.Month}-{form.Year}"),
        };
    }

    public static MemberModel Create(MemberEntity entity)
    {
        return new MemberModel
        {
            Id = Guid.Parse(entity.Id),
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageUrl = entity.ImageUrl,
            Phone = entity.PhoneNumber,
            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
            Title = entity.Title,
            Status = entity.Status,
            BirthDate = entity.BirthDate,
            Address = AddressFactory.Create(entity.Address),
        };
    }

    public static MemberModel CreateWithProjects(MemberEntity entity)
    {
        return new MemberModel
        {
            Id = Guid.Parse(entity.Id),
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageUrl = entity.ImageUrl,
            Phone = entity.PhoneNumber,
            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
            Title = entity.Title,
            Status = entity.Status,
            BirthDate = entity.BirthDate,
            Address = AddressFactory.Create(entity.Address),
            Projects = entity.Projects.Select(ProjectFactory.Create).ToList(),
        };
    }

    public static MemberEntity Create(MemberModel model)
    {
        return new MemberEntity
        {
            Id = model.Id.ToString(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            ImageUrl = model.ImageUrl,
            PhoneNumber = model.Phone,
            Title = model.Title,
            Status = model.Status,
            DateUpdated = model.DateUpdated,
            DateCreated = model.DateCreated,
            BirthDate = model.BirthDate,
            Address = AddressFactory.Create(model.Address),
            Projects = model.Projects.Select(ProjectFactory.Create).ToList()
        };
    }

    public static AddMemberFormModel CreateRegistrationUpdateForm(MemberModel model)
    {
        var form = new AddMemberFormModel();

        form.FirstName = model.FirstName;
        form.LastName = model.LastName;
        form.Email = model.Email;
        form.ImageName = model.ImageUrl;
        form.Phone = model.Phone;
        form.Title = model.Title ?? "";
        form.Street = model.Address.Street;
        form.ZipCode = model.Address.ZipCode;
        form.City = model.Address.City;
        form.Country = model.Address.Country;
        form.Day = model.BirthDate.Day;
        form.Month = model.BirthDate.Month;
        form.Year = model.BirthDate.Year;
   
        return form;
    }

    public static MemberEntity Update(EditMemberFormModel form, MemberEntity member)
    {
        member.FirstName = form.FirstName;
        member.LastName = form.LastName;
        member.Email = form.Email;
        member.PhoneNumber = form.Phone;
        member.BirthDate = DateOnly.Parse($"{form.Year}-{form.Month}-{form.Day}");
        member.Title = form.Title;
        member.Status = form.Status;

        if (!string.IsNullOrEmpty(form.ImageName))
        {
            member.ImageUrl = form.ImageName;
        }

        return member;
    }
}
