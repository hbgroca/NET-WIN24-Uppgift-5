using Business.Dtos;
using Business.Models;
using Data.Entities;
using Shared.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;
public interface IClientService
{
    Task<ClientModel> CreateClientAsync(AddClientFormModel form);
    Task<IEnumerable<ClientModel>> GetAllClientsAsync();
    Task<ClientModel> GetClientAsync(Expression<Func<ClientEntity, bool>> expression);
    Task<IEnumerable<ClientModel>> GetClientsAsync(Expression<Func<ClientEntity, bool>> expression);
}