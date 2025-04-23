using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Models;

namespace Parent_Teacher.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        // Add other DbSets here
    }
}
