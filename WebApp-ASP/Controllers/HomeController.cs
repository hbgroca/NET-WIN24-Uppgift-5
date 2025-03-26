using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        return View();
    }
}
