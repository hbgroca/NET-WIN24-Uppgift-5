using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }
}
