using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Parent
{
    public class ParentModel : PageModel
    {
        private readonly AppDbContext _context;

        public ParentModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Student> Students { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                students = students.Where(s =>
                    s.StudentID.Contains(SearchString) ||
                    s.FirstName.Contains(SearchString) ||
                    s.LastName.Contains(SearchString) ||
                    s.Course.Contains(SearchString) ||
                    s.Section.Contains(SearchString)
                );
            }

            Students = await students
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}