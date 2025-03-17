using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers
{
    public class AdminController(IClientService clientService) : Controller
    {
        private readonly IClientService _clientService = clientService;

        [Route("Admin")]
        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [Route("members")]
        public IActionResult Members()
        {
            ViewData["Title"] = "Members";
            return View();
        }

        [Route("clients")]
        public async Task<IActionResult> Clients()
        {
            ViewData["Title"] = "Clients";

            return View();
        }


        
    }
}
