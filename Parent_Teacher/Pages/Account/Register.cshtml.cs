using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Parent_Teacher.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;

        public RegisterModel(AppDbContext context)
        {
            _context = context;
            Message = string.Empty; // Initialize Message to avoid null error
        }

        [BindProperty]
        public RegisterInput Input { get; set; } = new RegisterInput(); // Initializes Input to avoid null error

        // Initialize Message here as well to avoid null errors.
        public string Message { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Please fill all the fields.";
                return Page();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == Input.Email);
            if (existingUser != null)
            {
                Message = "Email is already registered.";
                return Page();
            }

            var newUser = new User
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = Input.Password,
                Role = Input.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetString("UserEmail", newUser.Email);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            return newUser.Role switch
            {
                "Teacher" => RedirectToPage("/Dashboards/Teacher"),
                "Parent" => RedirectToPage("/Dashboards/Parent"),
                _ => RedirectToPage("/Account/Login")
            };
        }

        public class RegisterInput
        {
            [Required(ErrorMessage = "Name is required.")]
            public string Name { get; set; } = string.Empty; // Initialize to avoid null

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid Email Address")]
            public string Email { get; set; } = string.Empty; // Initialize to avoid null

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; } = string.Empty; // Initialize to avoid null

            [Required(ErrorMessage = "Role is required.")]
            public string Role { get; set; } = string.Empty; // Initialize to avoid null
        }
    }
}
