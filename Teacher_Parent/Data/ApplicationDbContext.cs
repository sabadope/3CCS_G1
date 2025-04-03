using Microsoft.EntityFrameworkCore;
using Teacher_Parent.Models;  // Import the User model

namespace Teacher_Parent.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // Represents the Users table in the DB
    }
}
