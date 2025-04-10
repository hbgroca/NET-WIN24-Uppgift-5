using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp_ASP.Models;

namespace WebApp_ASP.Controllers;

[Authorize]
public class HomeController(IProjectService projectService, IMemberService memberService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IMemberService _memberService = memberService;
    ProjectsPageViewModel viewModel = new();

    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        return View();
    }

    [Authorize]
    [Route("projects")]
    public async Task<IActionResult> Projects()
    {
        ViewData["Title"] = "Projects";
        var memberList = await _memberService.GetAllMembersAsync();
        viewModel._projectViewModel.MembersInDb = memberList.ToList();
        viewModel.Projects = await _projectService.GetAllProjectsAsync();
        return View(viewModel);
    }

    [Authorize]
    [Route("clients")]
    public IActionResult Clients()
    {
        ViewData["Title"] = "Clients";
        return View();
    }
}
