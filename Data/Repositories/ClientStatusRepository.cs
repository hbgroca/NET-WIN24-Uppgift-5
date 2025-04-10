using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ClientStatusRepository(DataContext context) : BaseRepository<ClientStatusEntity>(context), IClientStatusRepository
{
}
