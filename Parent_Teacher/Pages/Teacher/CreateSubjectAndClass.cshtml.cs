using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Teacher
{
    public class CreateSubjectAndClassModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateSubjectAndClassModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubjectClass SubjectClass { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.SubjectClasses.Add(SubjectClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Teacher/CreateStudent"); // Or wherever you'd like
        }
    }
}
