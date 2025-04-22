using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IAddressService addressService, IImageServices imageServices, INotificationSerivces notificationSerivces) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAddressService _addressService = addressService;
    private readonly IImageServices _imageServices = imageServices;
    private readonly INotificationSerivces _notificationsServices = notificationSerivces;

    // Create
    public async Task<ClientModel> CreateClientAsync(AddClientFormModel form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing.");
            return null!;
        }

        try
        {
            // Begin a new transaction
            await _clientRepository.BeginTransactionAsync();

            // Store image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var imagePath = await _imageServices.Create(form.ProfilePicture, "clients");
                if (!string.IsNullOrEmpty(imagePath))
                    form.ImageName = imagePath;
            }
            else
                form.ImageName = $"/images/defaultmember.png";

            // Remap with factory
            var clientEntity = ClientFactory.Create(form);
            clientEntity.Id = Guid.NewGuid();

            // Create the address
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
            {
                Debug.WriteLine("An error occurred while creating the client: ");
                await _clientRepository.RollbackTransactionAsync();
                return null!;
            }

            clientEntity.AddressId = address.Id;

            // Set the date created and updated
            clientEntity.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            clientEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Set the status
            clientEntity.Status = "Active";

            // Create the client in dbcontext
            await _clientRepository.CreateAsync(clientEntity);

            // Save the changes
            var result = await _clientRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the client");

            // Commit the transaction
            await _clientRepository.CommitTransactionAsync();

            // Send notifcations to other team members
            if (result > 0)
            {
                string Message = $"New Client: {clientEntity.ClientName}!";
                await _notificationsServices.AddNotificationAsync(1, Message, clientEntity.Id.ToString(), clientEntity.ImageUrl!, 1);
            }

            // Return the client
            return ClientFactory.Create(clientEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while creating the client: ", ex);
            _imageServices.Delete(form.ImageName!);
            await _clientRepository.RollbackTransactionAsync();
        }


        return null!;
    }


    // Read
    public async Task<ClientModel> GetClientAsync(Expression<Func<ClientEntity, bool>> expression)
    {
        var client = await _clientRepository.GetOneAsync(expression);
        if (client == null)
            return null!;
        return ClientFactory.Create(client);
    }

    public async Task<IEnumerable<ClientModel>> GetClientsAsync(Expression<Func<ClientEntity, bool>> expression)
    {
        var clients = await _clientRepository.GetAllAsync(expression);
        if (clients == null)
            return null!;
        return clients.Select(ClientFactory.Create);
    }

    public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        if (clients == null)
            return null!;

        return clients.Select(ClientFactory.Create);
    }

    // Update

    public async Task<bool> UpdateClient(EditClientFormModel form)
    {
        if (form == null)
        {
            Debug.WriteLine("EditClientFormModel missing.");
            return false;
        }

        try
        {
            // Begin a new transaction
            await _clientRepository.BeginTransactionAsync();
            // Get the client
            var client = await _clientRepository.GetOneAsync(x => x.Id == form.Id);

            if (client == null)
            {
                Debug.WriteLine("Client not found.");
                await _clientRepository.RollbackTransactionAsync();
                return false;
            }

            // Update image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var imagePath = await _imageServices.Update(form.ProfilePicture, "clients", client.ImageUrl!);
                if (!string.IsNullOrEmpty(imagePath))
                    form.ImageName = imagePath;
            }

            // Remap with factory
            var clientEntity = ClientFactory.Update(form, client);

            // Create the address
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
            {
                Debug.WriteLine("An error occurred while updating the client: ");
                await _clientRepository.RollbackTransactionAsync();
                return false;
            }

            clientEntity.AddressId = address.Id;

            // Set the date updated
            clientEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Update the client in dbcontext
            _clientRepository.Update(clientEntity);

            // Save the changes
            var result = await _clientRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the client");

            // Commit the transaction
            await _clientRepository.CommitTransactionAsync();

            // Send notifcations to other team members
            if (result > 0)
            {
                string Message = $"Client: {clientEntity.ClientName} was updated.";
                await _notificationsServices.AddNotificationAsync(1, Message, clientEntity.Id.ToString(), clientEntity.ImageUrl!, 1);
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating the client: ", ex.Message);
            _imageServices.Delete(form.ImageName!);
            return false;
        }
    }

    // Delete
    public async Task<bool> Delete(Guid id)
    {
        // Begin transaction
        await _clientRepository.BeginTransactionAsync();

        try
        {
            // Get the entity
            var entity = await _clientRepository.GetOneAsync(x => x.Id == id);
            if (entity == null)
                return false;

            // Delete from dbset
            _clientRepository.Delete(entity);

            // Save changes
            var save = await _clientRepository.SaveAsync();
            if (save == 0)
                return false;

            // Commit transaction
            await _clientRepository.CommitTransactionAsync();

            // Remove image
            _imageServices.Delete(entity.ImageUrl!);

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            // Rollback transaction if error
            await _clientRepository.RollbackTransactionAsync();
            return false;
        }
    }
}
