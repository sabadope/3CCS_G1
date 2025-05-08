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
        public List<Student> TopPerformers { get; set; }

        public TeacherModel(AppDbContext context)
        {
            _context = context;
            // Initialize lists to prevent null reference exceptions
            TopPerformers = new List<Student>();
            RecentStudents = new List<Student>();
            MonthlyCounts = new List<MonthlyStudentCount>();
            DashboardData = new TeacherDashboard();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var role = HttpContext.Session.GetString("UserRole");

                if (string.IsNullOrEmpty(role) || role != "Teacher")
                {
                    return RedirectToPage("/Account/Login");
                }

                // Get dashboard data
                DashboardData = new TeacherDashboard
                {
                    StudentCount = await _context.Students.CountAsync(),
                    CourseCount = await _context.CourseSections
                        .Select(cs => cs.CourseName)
                        .Distinct()
                        .CountAsync(),
                    SectionCount = await _context.CourseSections
                        .Select(cs => cs.SectionName)
                        .Distinct()
                        .CountAsync()
                };

                // Get recent students
                RecentStudents = await _context.Students
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                // Get top performers - handle empty case
                var allStudents = await _context.Students.ToListAsync();

                // Calculate overall average for each student
                foreach (var student in allStudents)
                {
                    var averages = new List<decimal>();
                    // Handle nullable decimals properly
                    if (student.TotalAverage.HasValue && student.TotalAverage.Value > 0)
                        averages.Add(student.TotalAverage.Value);

                    if (student.TotalAverage2.HasValue && student.TotalAverage2.Value > 0)
                        averages.Add(student.TotalAverage2.Value);

                    if (student.TotalAverage3.HasValue && student.TotalAverage3.Value > 0)
                        averages.Add(student.TotalAverage3.Value);

                    student.OverallAverage = averages.Any() ? averages.Average() : 0;
                }

                TopPerformers = allStudents
                    .OrderByDescending(s => s.OverallAverage)
                    .Take(5)
                    .ToList();

                // Get monthly student counts
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
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in OnGetAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return RedirectToPage("/Error");
            }
        }
    }

    public class TeacherDashboard
    {
        public int StudentCount { get; set; }
        public int CourseCount { get; set; }
        public int SectionCount { get; set; }
    }

    public class MonthlyStudentCount
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }
}