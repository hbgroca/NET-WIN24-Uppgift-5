using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class MemberFactory
{
    public static MemberEntity Create(MemberRegistrationform form)
    {
        return new MemberEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            ImageUrl = form.ImageUrl,
            Phone = form.Phone,
            Title = form.Title,
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
            Title = entity.Title,
            BirthDate = entity.BirthDate,
            Address = AddressFactory.Create(entity.Address),
            Projects = entity.Projects.Select(ProjectFactory.Create).ToList()
        };
    }
}
