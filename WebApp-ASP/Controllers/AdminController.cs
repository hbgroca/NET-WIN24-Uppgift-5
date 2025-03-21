using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp_ASP.Models;

namespace WebApp_ASP.Controllers
{
    public class AdminController(IProjectService projectService) : Controller
    {
        private readonly IProjectService _projectService = projectService;

        [Route("Admin")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [Route("projects")]
        public async Task<IActionResult> Projects()
        {
            ViewData["Title"] = "Projects";
            ProjectsPageViewModel viewModel = new();
            viewModel.Projects = await _projectService.GetAllProjectsAsync();
            return View(viewModel);
        }

        [Route("members")]
        public IActionResult Members()
        {
            ViewData["Title"] = "Members";
            return View();
        }

        [Route("clients")]
        public IActionResult Clients()
        {
            ViewData["Title"] = "Clients";

            return View();
        }
    }
}
