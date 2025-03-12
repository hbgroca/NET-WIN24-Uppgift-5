using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class ClientFactory
{
    public static ClientEntity Create(ClientRegistrationform form)
    {
        return new ClientEntity
        {
            ImageUrl = form.ImageUrl,
            ClientName = form.ClientName,
            Email = form.Email,
            Phone = form.Phone,
        };
    }

    public static ClientModel Create(ClientEntity entity)
    {
        return new ClientModel
        {
            Id = entity.Id,
            ImageUrl = entity.ImageUrl,
            ClientName = entity.ClientName,
            Email = entity.Email,
            Phone = entity.Phone,
            Address = AddressFactory.Create(entity.Address),
            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
            Projects = entity.Projects.Select(ProjectFactory.Create).ToList(),
        };
    }

    public static ClientEntity Create(ClientModel model)
    {
        return new ClientEntity
        {
            Id = model.Id,
            ImageUrl = model.ImageUrl,
            ClientName = model.ClientName,
            Email = model.Email,
            Phone = model.Phone,
            Address = AddressFactory.Create(model.Address),
            DateCreated = model.DateCreated,
            DateUpdated = model.DateUpdated,
            Projects = model.Projects.Select(ProjectFactory.Create).ToList(),
        };
    }
}
