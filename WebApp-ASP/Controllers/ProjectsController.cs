using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class ProjectsController(IProjectService projectService) : Controller
    {
        private readonly IProjectService _projectService = projectService;

        public IActionResult Index()
        {
            ViewData["Title"] = "Projects";
            return View();
        }
    }
}
