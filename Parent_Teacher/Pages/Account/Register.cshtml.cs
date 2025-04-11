using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string Message { get; set; } = string.Empty;

        public class InputModel
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Invalid form.";
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                Message = "Passwords do not match.";
                return Page();
            }

            if (string.IsNullOrEmpty(Input.Role))
            {
                Message = "Please select a role.";
                return Page();
            }

            var user = new User
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = Input.Password,
                Role = Input.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToPage("/Account/Login"); // Next step!
        }
    }
}
