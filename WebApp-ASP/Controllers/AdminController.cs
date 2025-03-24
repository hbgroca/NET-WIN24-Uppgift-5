using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using WebApp_ASP.Models;

namespace WebApp_ASP.Controllers
{
    public class AdminController(IProjectService projectService,IMemberService memberService) : Controller
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMemberService _memberService = memberService;

        ProjectsPageViewModel viewModel = new();

        [Route("Admin")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [Route("projects")]
        public async Task<IActionResult> Projects(ProjectsPageViewModel viewModel)
        {
            ViewData["Title"] = "Projects";
            //ProjectsPageViewModel viewModel = new();
            var memberList = await _memberService.GetAllMembersAsync();
            viewModel._projectViewModel.MembersInDb = memberList.ToList();
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
