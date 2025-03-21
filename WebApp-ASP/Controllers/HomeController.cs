using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp_ASP.Models;

namespace WebApp_ASP.Controllers;

public class HomeController : Controller
{
    [Route("home")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        return View();
    }
}
