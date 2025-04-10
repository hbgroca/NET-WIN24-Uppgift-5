using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class MemberStatusRepository(DataContext context) : BaseRepository<MemberStatusEntity>(context), IMemberStatusRepository
{

}
