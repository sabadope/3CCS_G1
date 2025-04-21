using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parent_Teacher.Pages.Admin
{
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;

        public AdminModel(AppDbContext context)
        {
            _context = context;
        }

        public List<User> TeachersAndParents { get; set; } = new List<User>();
        public List<User> RecentUsers { get; set; } = new List<User>();

        public int TeacherCount { get; set; }
        public int ParentCount { get; set; }

        // Add this to pass role counts to Chart.js
        public Dictionary<string, int> RoleCounts { get; set; } = new();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            // Get Teachers and Parents
            TeachersAndParents = _context.Users
                .Where(u => u.Role == "Teacher" || u.Role == "Parent")
                .ToList();

            TeacherCount = TeachersAndParents.Count(u => u.Role == "Teacher");
            ParentCount = TeachersAndParents.Count(u => u.Role == "Parent");

            RecentUsers = TeachersAndParents
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToList();

            // Pie chart data: only Teacher and Parent
            RoleCounts = TeachersAndParents
                .GroupBy(u => u.Role)
                .ToDictionary(g => g.Key, g => g.Count());

            return Page();
        }
    }
}
