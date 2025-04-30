using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Teacher
{
    public class CreateCourseAndSectionModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateCourseAndSectionModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CourseSection CourseSection { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.CourseSections.Add(CourseSection);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Teacher/CreateStudent");
        }
    }
}
