using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Teacher
{
    public class EditStudentModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditStudentModel(AppDbContext context)
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);
            if (Student == null)
            {
                return RedirectToPage("/Teacher/Students");
            }

            PopulateCourseList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            PopulateCourseList();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var studentToUpdate = await _context.Students.FindAsync(Student.Id);
            if (studentToUpdate == null)
            {
                return RedirectToPage("/Teacher/Students");
            }

            studentToUpdate.FirstName = Student.FirstName;
            studentToUpdate.LastName = Student.LastName;
            studentToUpdate.Course = Student.Course;
            studentToUpdate.Section = Student.Section;

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
