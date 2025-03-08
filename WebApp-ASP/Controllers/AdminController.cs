using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class AdminController : Controller
    {
        [Route("Admin")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }
    }
}
