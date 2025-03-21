using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }
        [Route("signup")]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "Sign Up";
            return View();
        }
    }
}
