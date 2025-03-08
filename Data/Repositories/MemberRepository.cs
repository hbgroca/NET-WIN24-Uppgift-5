using Data.Entities;

namespace Data.Repositories;

public class MemberRepository(DataContext context) : BaseRepository<MemberEntity>(context)
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }
}