using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Restaurantproject.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult ChangeLanguage(string culter, string returnurl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culter)),
                new CookieOptions { Expires = DateTime.UtcNow.AddDays(1) }
                );

            return LocalRedirect(returnurl ?? "/");
        }
    }
}
