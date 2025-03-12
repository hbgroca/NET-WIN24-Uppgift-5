using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IAddressService
{
    Task<AddressModel> CreateAddressAsync(AddressRegistrationform form);
    Task<AddressModel> CreateAddressAsync(string street, string zipcode, string city, string country);
}