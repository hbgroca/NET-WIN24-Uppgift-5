using Business.Dtos;
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
    public async Task<ClientModel> CreateClientAsync(ClientRegistrationform form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing.");
            return null!;
        }

        // Begin a new transaction
        await _clientRepository.BeginTransactionAsync();
        try
        {
            // Remap with factory
            var clientEntity = ClientFactory.Create(form);
            clientEntity.Id = GenerateGuid.NewGuid();

            // Create the address
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
                throw new Exception("Error while creating the address");

            clientEntity.AddressId = address.Id;

            // Set the date created and updated
            clientEntity.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            clientEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

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

    // Delete

}
