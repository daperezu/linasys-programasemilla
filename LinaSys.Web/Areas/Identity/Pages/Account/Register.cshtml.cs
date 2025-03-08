using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using LinaSys.Auth.Application.Commands;
using LinaSys.Notification.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace LinaSys.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel(
    UserManager<Auth.Domain.Entities.User> userManager,
    MediatR.IMediator mediatR,
    ILogger<RegisterModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public Task OnGetAsync(string returnUrl = "")
    {
        ReturnUrl = returnUrl;
        return Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = "")
    {
        returnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            var (userCreated, errors) = await mediatR.Send(new RegisterUserCommand(Input.Username, Input.Email, Input.Password)).ConfigureAwait(false);

            if (userCreated is not null)
            {
                logger.LogInformation("User created a new account with password.");

                var code = await userManager.GenerateEmailConfirmationTokenAsync(userCreated);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userCreated.Id, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                await mediatR.Send(new SendEmailCommand(Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>."));

                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            }

            foreach (var error in errors!)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    public class InputModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirma tu contraseña")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Identificatión")]
        public string Username { get; set; }
    }
}
