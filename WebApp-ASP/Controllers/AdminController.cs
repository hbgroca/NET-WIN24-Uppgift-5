using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApp_ASP.Controllers
{
    public class AdminController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, INotificationSerivces notificationSerivces, IMemberService memberService) : Controller
    {
        private readonly SignInManager<MemberEntity> _signInManager = signInManager;
        private readonly UserManager<MemberEntity> _userManager = userManager;
        private readonly IAuthService _authService = authService;
        private readonly INotificationSerivces _notificationService = notificationSerivces;
        private readonly IMemberService _memberService = memberService;

        public IActionResult SignIn()
        {
            ViewData["Title"] = "Sign In";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("members")]
        public IActionResult Members()
        {
            ViewData["Title"] = "Members";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("status")]
        public IActionResult Status()
        {
            ViewData["Title"] = "Status";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(MemberLoginFormModel form, string returnUrl = "~/")
        {
            ViewData["Title"] = "Admin Sign In";
            ViewBag.Error = "";
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid login credentials";
                return View(form);
            }

            // Try login
            var isAuthenticated = await _authService.AuthenticateAdminAsync(form);
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login credentials";
            return View(form);
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


            //string redirectUrl = Url.Action("SignIn")!;
            string redirectUrl = Url.Action("ExternalSignInCallback", "Admin", new { returnUrl })!;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
        {
            returnUrl ??= Url.Content("SignIn");

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return LocalRedirect("SignIn");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");
                return LocalRedirect("SignIn");
            }

            // Check admin status
            var email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            var isAdmin = await _memberService.IsMemberAdmin($"ext_google_{email}");
            if (!isAdmin)
                return RedirectToAction("SignIn");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "User not registered";
            return LocalRedirect("SignIn");
        }



        [HttpPost]
        [Route("ConvertToAdmin/{userName}")]
        public async Task<IActionResult> ConvertToAdmin(string userName)
        {
            var entity = await _userManager.FindByNameAsync(userName);
            if(entity != null)
                await _userManager.AddToRoleAsync(entity, "Admin");

            await _signInManager.SignOutAsync();

            return RedirectToAction("SignIn", "Admin");
        }

        [HttpPost]
        [Route("ConvertToMember/{userName}")]
        public async Task<IActionResult> ConvertToMember(string userName)
        {
            var entity = await _userManager.FindByNameAsync(userName);
            if (entity != null)
                await _userManager.RemoveFromRoleAsync(entity, "Admin");

            await _signInManager.SignOutAsync();

            return RedirectToAction("SignIn", "Login");
        }
    }
}
