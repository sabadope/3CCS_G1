using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parent_Teacher.Pages.Dashboards
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

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            // Get all Teachers and Parents
            TeachersAndParents = _context.Users
                .Where(u => u.Role == "Teacher" || u.Role == "Parent")
                .ToList();

            TeacherCount = TeachersAndParents.Count(u => u.Role == "Teacher");
            ParentCount = TeachersAndParents.Count(u => u.Role == "Parent");

            // Get the 5 most recent accounts (Teacher or Parent)
            RecentUsers = _context.Users
                .Where(u => u.Role == "Teacher" || u.Role == "Parent")
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToList();

            return Page();
        }
    }
}
