using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Models;
using System.Collections.Generic;

namespace Parent_Teacher.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
