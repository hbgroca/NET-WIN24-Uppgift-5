using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;

namespace Data.Repositories
{
    public class AddressRepository(DataContext context) : BaseRepository<AddressEntity>(context), IAddressRepository
    {
    }
}
