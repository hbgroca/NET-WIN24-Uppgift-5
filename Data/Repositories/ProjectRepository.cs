using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }
}
