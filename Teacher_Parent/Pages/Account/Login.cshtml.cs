using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Teacher_Parent.Data;
using Teacher_Parent.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;

namespace Teacher_Parent.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public required string Email { get; set; }
        [BindProperty]
        public required string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError(string.Empty, "Both fields are required.");
                return Page();
            }

            // Check if user exists
            var user = await _context.Users
                .Where(u => u.Email == Email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Verify password (For simplicity, assuming you stored the hashed password correctly)
            if (!VerifyPasswordHash(Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // Logic for successful login (you can set a session, cookie, etc.)
            // Redirect to dashboard or home page
            return RedirectToPage("/Home/Index");
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Use the same hashing algorithm as during registration (e.g., PBKDF2)
            // For simplicity, this is a basic example and should be improved with actual secure practices
            var hashBytes = Convert.FromBase64String(storedHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            return computedHash.SequenceEqual(hashBytes.Skip(16).Take(32));
        }
    }
}
