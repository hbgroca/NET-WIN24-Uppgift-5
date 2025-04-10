using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp_ASP.Controllers
{
    public class LoginController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, INotificationSerivces notificationSerivces) : Controller
    {
        private readonly SignInManager<MemberEntity> _signInManager = signInManager;
        private readonly UserManager<MemberEntity> _userManager = userManager;
        private readonly IAuthService _authService = authService;
        private readonly INotificationSerivces _notificationService = notificationSerivces;
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


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            if(result)
                return LocalRedirect("~/");

            return View();
        }


        // External Sign In
        [HttpPost]
        public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
        {
            if (string.IsNullOrEmpty(provider))
            {
                ModelState.AddModelError("", "Invalid provider");
                return View("SignIn");
            }
               

            string redirectUrl = Url.Action("ExternalSignInCallback", "Login", new { returnUrl })!;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
        {
            returnUrl ??= Url.Content("~/");

            if(!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return View("SignIn");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");
                return RedirectToAction("SignIn");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded) { 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
                string userName = $"ext_{info.LoginProvider.ToLower()}_{email}";

                var user = new MemberEntity
                {
                    UserName = userName,
                    Email = email,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                    BirthDate = DateOnly.Parse(info.Principal.FindFirstValue(ClaimTypes.DateOfBirth) ?? "1900-01-01"),
                    PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone) ?? "",
                    Status = "Active",
                    Title = "Junior",
                    ImageUrl = "/images/defaultmember.png",
                    Address = new AddressEntity
                    {
                        Street = info.Principal.FindFirstValue(ClaimTypes.StreetAddress) ?? "",
                        ZipCode = info.Principal.FindFirstValue(ClaimTypes.PostalCode) ?? "",
                        City = info.Principal.FindFirstValue(ClaimTypes.StateOrProvince) ?? "",
                        Country = info.Principal.FindFirstValue(ClaimTypes.Country) ?? "",
                    }
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    // Send notification to members
                    string Message = $"Member {user.FirstName} {user.LastName} signed up!";
                    await _notificationService.AddNotificationAsync(3, Message, user.Id, user.ImageUrl!, 2);


                    identityResult = await _userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            return LocalRedirect("SignIn");
        }
    }
}
