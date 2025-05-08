using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;

namespace Parent_Teacher.Pages.Parent
{
    public class ViewChildModel : PageModel
    {
        private readonly AppDbContext _context;

        public ViewChildModel(AppDbContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

            if (Student == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}