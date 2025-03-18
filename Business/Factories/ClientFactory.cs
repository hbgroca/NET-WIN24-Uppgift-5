using Business.Dtos;
using Business.Models;
using Data.Entities;
using Shared.Models;

namespace Business.Factories;

public class ClientFactory
{
    public static ClientEntity Create(AddClientFormModel form)
    {
        return new ClientEntity
        {
            ImageUrl = form.ImageName,
            ClientName = form.Name,
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
            Status = entity.Status,
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
            Status = model.Status,
            Projects = model.Projects.Select(ProjectFactory.Create).ToList(),
        };
    }

    public static ClientEntity Update(EditClientFormModel form, ClientEntity client)
    {
        client.Status = form.Status;
        client.ClientName = form.Name;
        client.Email = form.Email;
        client.Phone = form.Phone;

        if (!string.IsNullOrEmpty(form.ImageName))
        {
            client.ImageUrl = form.ImageName;
        }
 
        return client;
    }
}
