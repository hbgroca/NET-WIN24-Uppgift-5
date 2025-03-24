using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IAddressService addressService) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IAddressService _addressService = addressService;

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

            // Remap with factory
            var clientEntity = ClientFactory.Create(form);
            clientEntity.Id = GenerateGuid.NewGuid();

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
            clientEntity.Status = "Alive";

            // Create the client in dbcontext
            await _clientRepository.CreateAsync(clientEntity);

            // Save the changes
            var result = await _clientRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the client");

            // Commit the transaction
            await _clientRepository.CommitTransactionAsync();

            // Return the client
            return ClientFactory.Create(clientEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while creating the client: ", ex);
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
            var imageUrl = client.ImageUrl;

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

            /// Remove image
            if (form.ProfilePicture is not null && imageUrl is not null)
            {
                var cutString = $"{Environment.CurrentDirectory}\\wwwroot\\uploaded\\clients\\{imageUrl.Substring(18)}";
                Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
                if (File.Exists(cutString))
                {
                    Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
                    File.Delete(cutString);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating the client: ", ex);
            return false;
        }
    }

    // Delete

}
