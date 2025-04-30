using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Teacher
{
    public class DeleteStudentModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteStudentModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return RedirectToPage("/Teacher/Students");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Student == null || Student.Id == 0)
            {
                return RedirectToPage("/Teacher/Students");
            }

            var studentToDelete = await _context.Students.FindAsync(Student.Id);

            if (studentToDelete == null)
            {
                return RedirectToPage("/Teacher/Students");
            }

            _context.Students.Remove(studentToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Teacher/Students");
        }
    }
}
