using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp_ASP.Controllers
{
    public class LoginController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        public IActionResult SignIn(string returnUrl = "~/")
        {
            ViewData["Title"] = "Sign In";
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(MemberLoginFormModel form, string returnUrl = "~/")
        {
            ViewData["Title"] = "Sign In";
            ViewBag.Error = "";
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid login credentials";
                return View(form);
            }

            // Try login
            var isAuthenticated = await _authService.AuthenticateAsync(form);
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login credentials";
            return View(form);
        }


        [Route("signup")]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "Sign Up";
            return View();
        }


        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(MemberSignUpFormModel form)
        {
            ViewData["Title"] = "Sign Up";
            ViewBag.Error = "";
            if (!form.TermsAndConditions)
                ModelState.AddModelError("TermsAndConditions", " ");
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid credentials";
                return View(form);
            }

            // Try login
            var wasSuccessfull = await _authService.SignUpAsync(form);
            if (wasSuccessfull)
            {
                return Redirect("~/");
            }

            ViewBag.Error = "Invalid credentials";
            return View(form);
        }


        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            if(result)
                return LocalRedirect("~/");

            return View();
        }
    }
}
