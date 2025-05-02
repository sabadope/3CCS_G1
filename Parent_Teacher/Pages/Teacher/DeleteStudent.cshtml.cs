using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Threading.Tasks;

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
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var student = await _context.Students.FindAsync(Student.Id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Students");
        }
    }
}
