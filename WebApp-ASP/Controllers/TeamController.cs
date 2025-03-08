using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class TeamController : Controller
    {
        [Route("teams")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Team";
            return View();
        }
    }
}
