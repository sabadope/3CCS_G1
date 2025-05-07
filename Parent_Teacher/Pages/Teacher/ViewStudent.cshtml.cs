using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Teacher
{
    public class ViewStudentModel : PageModel
    {
        private readonly AppDbContext _context;

        public ViewStudentModel(AppDbContext context)
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
    }
}
