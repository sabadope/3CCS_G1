using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Teacher
{
    public class StudentsModel : PageModel
    {
        private readonly AppDbContext _context;

        public StudentsModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get; set; }

        public DateTime CreatedAt { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }



        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Ascending { get; set; } = true;



        public async Task OnGetAsync()
        {
            var studentsQuery = _context.Students.AsQueryable();

            // Apply search filtering
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                string lowered = SearchQuery.ToLower();
                studentsQuery = studentsQuery.Where(s =>
                    s.FirstName.ToLower().Contains(lowered) ||
                    s.LastName.ToLower().Contains(lowered));
            }


            // Apply sorting
            studentsQuery = SortBy switch
            {
                "StudentID" => Ascending ? studentsQuery.OrderBy(s => s.StudentID) : studentsQuery.OrderByDescending(s => s.StudentID),
                "FirstName" => Ascending ? studentsQuery.OrderBy(s => s.FirstName) : studentsQuery.OrderByDescending(s => s.FirstName),
                "LastName" => Ascending ? studentsQuery.OrderBy(s => s.LastName) : studentsQuery.OrderByDescending(s => s.LastName),
                "Course" => Ascending ? studentsQuery.OrderBy(s => s.Course) : studentsQuery.OrderByDescending(s => s.Course),
                "Section" => Ascending ? studentsQuery.OrderBy(s => s.Section) : studentsQuery.OrderByDescending(s => s.Section),
                "CreatedAt" => Ascending ? studentsQuery.OrderBy(s => s.CreatedAt) : studentsQuery.OrderByDescending(s => s.CreatedAt),
                _ => studentsQuery.OrderByDescending(s => s.CreatedAt)
            };

            Students = await studentsQuery.ToListAsync();
        }


    }
}
