using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinaSys.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel(UserManager<Auth.Domain.Entities.User> userManager)
    : PageModel
{
    public string Email { get; set; }

    public async Task<IActionResult> OnGetAsync(string email, string returnUrl = "")
    {
        if (email == null)
        {
            return RedirectToPage("/Index");
        }

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound($"Unable to load user with email '{email}'.");
        }

        Email = email;

        return Page();
    }
}
