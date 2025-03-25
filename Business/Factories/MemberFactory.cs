using Business.Models;
using Data.Entities;

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
            Phone = form.Phone,
            Title = form.Title,
            Status = "Active",
            BirthDate = DateOnly.Parse($"{form.Day}-{form.Month}-{form.Year}"),
        };
    }

    public static MemberModel Create(MemberEntity entity)
    {
        return new MemberModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageUrl = entity.ImageUrl,
            Phone = entity.Phone,
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
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            ImageUrl = entity.ImageUrl,
            Phone = entity.Phone,
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
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            ImageUrl = model.ImageUrl,
            Phone = model.Phone,
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
        return new AddMemberFormModel
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            ImageName = model.ImageUrl,
            Phone = model.Phone,
            Title = model.Title,
            Day = model.BirthDate.Day,
            Month = model.BirthDate.Month,
            Year = model.BirthDate.Year,
            Street = model.Address.Street,
            ZipCode = model.Address.ZipCode,
            City = model.Address.City,
            Country = model.Address.Country
        };
    }

    public static MemberEntity Update(EditMemberFormModel form, MemberEntity member)
    {
        member.FirstName = form.FirstName;
        member.LastName = form.LastName;
        member.Email = form.Email;
        member.Phone = form.Phone;
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
