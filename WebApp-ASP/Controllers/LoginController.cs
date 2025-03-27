using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApp_ASP.Controllers
{
    public class LoginController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly string[] ErrorMessages = [
            "Oops! You missed some fields.",
            "Hey, don’t forget to fill out everything!",
            "Almost there! Just complete all the fields.",
            "Some fields are still empty. Give them some love!",
            "Whoa there, you forgot something!",
            "This form isn't going to fill itself!",
            "Nice try, but you need to complete all fields first!",
            "Do I look like a mind reader? Fill in the blanks!",
            "Please fill in all required fields.",
            "All fields must be completed before submitting.",
            "Missing information: Ensure all fields are filled out.",
            "Required fields cannot be left empty."
            ];
        private readonly string[] InvalidSingInErrorMessages = [
            "Invalid credentials. Please try again.",
            "Your login details don’t match our records.",
            "Incorrect username or password. Please try again.",
            "The credentials you entered are not valid.",
            "Login failed. Please check your username and password.",
            "Your account details could not be verified.",
            "Invalid login attempt. Please try again.",
            ];
        Random rnd = new Random();

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
                ViewBag.Error = ErrorMessages[rnd.Next(0, ErrorMessages.Count())];
                return View(form);
            }

            // Try login
            var isAuthenticated = await _authService.AuthenticateAsync(form);
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = InvalidSingInErrorMessages[rnd.Next(0, InvalidSingInErrorMessages.Count())];
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
                ViewBag.Error = ErrorMessages[rnd.Next(0, ErrorMessages.Count())];
                return View(form);
            }

            // Try login
            var wasSuccessfull = await _authService.SignUpAsync(form);
            if (wasSuccessfull)
            {
                return Redirect("~/");
            }

            ViewBag.Error = "Something went wrong, try again later";
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
