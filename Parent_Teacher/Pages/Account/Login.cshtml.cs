using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();


        public string Message { get; set; } = "";

        public class InputModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Message = "Invalid login attempt.";
                return Page();
            }

            // Admin account check
            if (Input.Email == "admin@gmail.com" && Input.Password == "admin1230")
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("UserEmail", Input.Email);

                return RedirectToPage("/Dashboards/Admin");
            }

            // Check user from DB
            var user = _context.Users.FirstOrDefault(u => u.Email == Input.Email && u.Password == Input.Password);

            if (user == null)
            {
                Message = "Invalid credentials.";
                return Page();
            }

            // Store session info
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Redirect based on role
            return user.Role switch
            {
                "Teacher" => RedirectToPage("/Dashboards/Teacher"),
                "Parent" => RedirectToPage("/Dashboards/Parent"),
                _ => RedirectToPage("/Account/Login")
            };
        }
    }
}
