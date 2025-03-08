using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class ProjectsController : Controller
    {
        [Route("projects")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Projects";
            return View();
        }
    }
}
