using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class AddressService(IAddressRepository addressRepository) : IAddressService
{
    private readonly IAddressRepository _addressRepository = addressRepository;

    // Create
    /// Create a new address. If the address already exists, returns the existing address.
    public async Task<AddressModel> CreateAddressAsync(AddressRegistrationform form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing.");
            return null!;
        }

        // Check if the address already exists
        var exists = await GetAddressAsync(
            c =>
            c.Street == form.Street &&
            c.ZipCode == form.ZipCode &&
            c.City == form.City &&
            c.Country == form.Country
            );
        if (exists != null)
            return exists;


        // Begin a new transaction
        await _addressRepository.BeginTransactionAsync();
        try
        {
            // Remap with factory
            var address = AddressFactory.Create(form);

            // Create the address in dbcontext
            await _addressRepository.CreateAsync(address);

            // Save the changes
            var result = await _addressRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the address");

            // Commit the transaction
            await _addressRepository.CommitTransactionAsync();

            // Get the address from db
            var updatedEntity = await _addressRepository.GetOneAsync(
                c =>
                c.Street == form.Street &&
                c.ZipCode == form.ZipCode &&
                c.City == form.City &&
                c.Country == form.Country
                );
            if (updatedEntity is null)
                throw new Exception("Error while saving the address");

            return AddressFactory.Create(updatedEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while creating the address: ", ex);
            await _addressRepository.RollbackTransactionAsync();
        }

        return null!;
    }

    public async Task<AddressModel> CreateAddressAsync(string street, string zipcode, string city, string country)
    {
        var addressform = AddressFactory.Create(street, zipcode, city, country);
        return await CreateAddressAsync(addressform);
    }

    // Read
    private async Task<AddressModel> GetAddressAsync(Expression<Func<AddressEntity, bool>> expression)
    {
        if (expression == null)
        {
            Debug.WriteLine("Invalid expression.");
            return null!;
        }
        // Get the address from db
        var entity = await _addressRepository.GetOneAsync(expression);
        if (entity == null)
        {
            Debug.WriteLine("Address not found.");
            return null!;
        }
        // Remap the entity to model
        var model = AddressFactory.Create(entity);
        return model;
    }
}
