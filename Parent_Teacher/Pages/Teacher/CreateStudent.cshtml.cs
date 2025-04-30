using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Teacher
{
    public class CreateStudentModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateStudentModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public List<SelectListItem> CourseList { get; set; }

        public void PopulateCourseList()
        {
            CourseList = _context.CourseSections
                .Select(cs => cs.CourseName)
                .Distinct()
                .Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                }).ToList();
        }

        public IActionResult OnGet()
        {
            PopulateCourseList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PopulateCourseList();

            if (!ModelState.IsValid)
                return Page();

            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Teacher/Students");
        }


        public async Task<JsonResult> OnGetSectionsAsync(string course)
        {
            var sections = await _context.CourseSections
                .Where(cs => cs.CourseName == course)
                .Select(cs => cs.SectionName)
                .Distinct()
                .ToListAsync();

            return new JsonResult(sections);
        }
    }
}
