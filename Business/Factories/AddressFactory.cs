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
            Debug.WriteLine("! Address Factory: Registrationform missing");
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

    public static AddressRegistrationform Create(string street, string zipcode, string city, string country)
    {
        if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(zipcode) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
        {
            Debug.WriteLine("! Address Factory: Required inputs missing");
            return null!;
        }
        return new AddressRegistrationform
        {
            Street = street,
            ZipCode = zipcode,
            City = city,
            Country = country,
        };
    }

    public static AddressModel Create(AddressEntity entity) 
    {
        if (entity == null)
        {
            Debug.WriteLine("! Address Factory: Entity missing");
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

    public static AddressEntity Create(AddressModel model)
    {
        if (model == null)
        {
            Debug.WriteLine("! Address Factory: Entity missing");
            return null!;
        }
        return new AddressEntity
        {
            Id = model.Id,
            Street = model.Street,
            ZipCode = model.ZipCode,
            City = model.City,
            Country = model.Country,
        };
    }
}
