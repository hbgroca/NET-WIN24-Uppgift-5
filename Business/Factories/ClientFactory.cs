using Business.Dtos;
using Business.Models;
using Data.Entities;

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
        ClientModel model = new();
        model.Id = entity.Id;
        model.ImageUrl = entity.ImageUrl;
        model.ClientName = entity.ClientName;
        model.Email = entity.Email;
        model.Phone = entity.Phone;
        model.Address = AddressFactory.Create(entity.Address);
        model.DateCreated = entity.DateCreated;
        model.DateUpdated = entity.DateUpdated;
        model.Status = entity.Status;
        model.ProjectCount = entity.Projects.Count();

        return model;
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
