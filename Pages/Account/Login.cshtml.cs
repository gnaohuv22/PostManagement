using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebApplication1.Settings;

namespace WebApplication1.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthentication _authentication;

        [BindProperty]
        public Credential Credential { get; set; } = new();

        [BindProperty]
        public string? ErrorMessage { get; set; } = string.Empty;

        public LoginModel(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _authentication.Login(Credential.Email, Credential.Password);
            if (result != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Email),
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString())
                };
                HttpContext.Session.SetString("Email", result.Email);

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Email or password not correct or invalid.";
                return Page();
            }
        }
    }
}
