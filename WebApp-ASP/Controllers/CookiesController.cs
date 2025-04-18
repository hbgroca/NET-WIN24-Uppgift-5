using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp_ASP.Controllers;
public class CookiesController : Controller
{
    [Route("privacypolicy")]
    public IActionResult PrivacyPolicy()
    {
        return View();
    }


    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsentModel consent)
    {
        // Set essential cookies
        Response.Cookies.Append("SessionCookie", "Essential", new CookieOptions
        {
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(365),
        });

        if (consent == null)
            return BadRequest();

        // Set non-essential cookies based on user consent
        if (consent.Functional)
        {
            Response.Cookies.Append("FunctionalCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(90),
                SameSite = SameSiteMode.Lax,
                Path = "/",
            });
        }
        else
        {
            Response.Cookies.Delete("FunctionalCookie");
        }

        //if (consent.Analytics)
        //{
        //    Response.Cookies.Append("AnalyticsCookie", "Non-Essential", new CookieOptions
        //    {
        //        IsEssential = false,
        //        Expires = DateTimeOffset.UtcNow.AddDays(90),
        //        SameSite = SameSiteMode.Lax,
        //        Path = "/",
        //    });
        //}
        //else
        //{
        //    Response.Cookies.Delete("AnalyticsCookie");
        //}

        //if (consent.Marketing)
        //{
        //    Response.Cookies.Append("MarketingCookie", "Non-Essential", new CookieOptions
        //    {
        //        IsEssential = false,
        //        Expires = DateTimeOffset.UtcNow.AddDays(90),
        //        SameSite = SameSiteMode.Lax,
        //        Path = "/",
        //    });
        //}
        //else
        //{
        //    Response.Cookies.Delete("MarketingCookie");
        //}

        // Set in JS
        //Response.Cookies.Append("cookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        //{
        //    IsEssential = true,
        //    Expires = DateTimeOffset.UtcNow.AddDays(90),
        //    SameSite = SameSiteMode.Lax,
        //    Path = "/",
        //});


        return Ok();
    }
}
