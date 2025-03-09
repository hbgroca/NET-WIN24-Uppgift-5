using Business.Dtos;
using Business.Models;
using Data.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class AddressFactory
{
    public static AddressRegistrationform Create()
    {
        return new AddressRegistrationform();
    }

    public static AddressEntity Create(AddressRegistrationform form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing");
            return null!;
        }
        return new AddressEntity
        {
            Street = form.Street,
            ZipCode = form.ZipCode,
            City = form.City,
            Country = form.Country,
        };
    }

    public static AddressModel Create(AddressEntity entity) 
    {
        if (entity == null)
        {
            Debug.WriteLine("Entity missing");
            return null!;
        }
        return new AddressModel
        {
            Id = entity.Id,
            Street = entity.Street,
            ZipCode = entity.ZipCode,
            City = entity.City,
            Country = entity.Country,
        };
    }
}
