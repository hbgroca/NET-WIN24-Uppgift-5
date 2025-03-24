using Business.Interfaces;
using Business.Models;

namespace WebApp_ASP.Models
{
    public class ProjectsPageViewModel()
    {
        public ProjectViewModel _projectViewModel = new();

        public IEnumerable<ProjectModel> Projects = [];
        public IEnumerable<ProjectModel> ProjectsStarted()
        {
            var list = Projects.Where(x => x.IsCompleted == false).ToList();
            return list ?? [];
        }
        public IEnumerable<ProjectModel> ProjectsCompleted()
        {
            var list = Projects.Where(x => x.IsCompleted == true).ToList();
            return list ?? [];
        }

        public int ProjectsStartedCount() => ProjectsStarted().Count();
        public int ProjectsCompletedCount() => ProjectsCompleted().Count();
    }
}
