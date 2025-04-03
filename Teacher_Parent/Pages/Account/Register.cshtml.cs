using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Teacher_Parent.Data;
using Teacher_Parent.Models;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Teacher_Parent.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public RegisterModel(ApplicationDbContext db) { _db = db; }

        [BindProperty]
        public required string Username { get; set; }

        [BindProperty]
        public required string Email { get; set; }

        [BindProperty]
        public required string Password { get; set; }

        [BindProperty]
        public required string ConfirmPassword { get; set; }

        public IActionResult OnPost()
        {
            // Validate password confirmation
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return Page();
            }

            // Hash the password before saving it
            string passwordHash = HashPassword(Password);

            // Create a new user object
            var newUser = new User
            {
                Username = Username,
                Email = Email,
                PasswordHash = passwordHash
            };

            // Add user to the database
            _db.Users.Add(newUser);
            _db.SaveChanges();

            return RedirectToPage("Login");
        }

        // Method to hash the password using KeyDerivation
        private string HashPassword(string password)
        {
            // Generate a salt value
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a hash value from the password and the salt
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Return the hashed password along with the salt (could be stored together)
            return hashedPassword;
        }
    }
}
