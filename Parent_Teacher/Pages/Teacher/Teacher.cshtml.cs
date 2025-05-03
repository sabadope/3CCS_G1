using Parent_Teacher.Data;
using Parent_Teacher.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Parent_Teacher.Pages.Teacher
{
    public class TeacherModel : PageModel
    {
        private readonly AppDbContext _context;

        public TeacherDashboard DashboardData { get; set; }
        public List<MonthlyStudentCount> MonthlyCounts { get; set; }


        public List<Student> RecentStudents { get; set; }


        public TeacherModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Teacher")
            {
                return RedirectToPage("/Account/Login");
            }


            DashboardData = new TeacherDashboard
            {
                StudentCount = await _context.Students.CountAsync(),

                // Updated: Count unique course names from CourseSections table
                CourseCount = await _context.CourseSections
                    .Select(cs => cs.CourseName)
                    .Distinct()
                    .CountAsync(),

                // Updated: Count unique section names from CourseSections table
                SectionCount = await _context.CourseSections
                    .Select(cs => cs.SectionName)
                    .Distinct()
                    .CountAsync()
            };

            // NEW: Get recent students
            RecentStudents = await _context.Students
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .ToListAsync();

            int year = DateTime.Now.Year;

            var studentData = await _context.Students
                .Where(s => s.CreatedAt.Year == year)
                .GroupBy(s => s.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            MonthlyCounts = Enumerable.Range(1, 12)
                .Select(m => new MonthlyStudentCount
                {
                    Month = monthNames[m - 1],
                    Count = studentData.FirstOrDefault(d => d.Month == m)?.Count ?? 0
                })
                .ToList();




            return Page();
        }
    }
}
