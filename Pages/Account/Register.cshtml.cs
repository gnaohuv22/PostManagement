using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.Settings;

namespace WebApplication1.Pages.Account
{
    public class RegisterInput
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterModel : PageModel
    {
        private readonly IAuthentication _authentication;
        public RegisterModel(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        [BindProperty]
        public RegisterInput Register { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = new AppUser
            {
                Email = Register.Email,
                Password = Register.Password,
                Address = "1",
                Fullname = "a"
            };
            var result = await _authentication.Register(user);
            if (result)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid registration attempt.";
                return Page();
            }
        }
    }
}
