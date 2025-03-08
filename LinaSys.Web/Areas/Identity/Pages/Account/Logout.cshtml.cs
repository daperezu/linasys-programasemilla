using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinaSys.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel(SignInManager<Auth.Domain.Entities.User> signInManager, ILogger<LogoutModel> logger)
    : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string returnUrl = "")
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out.");
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToPage();
        }
    }
}
