using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class MemberRepository(DataContext context) : BaseRepository<MemberEntity>(context), IMemberRepository
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }
}