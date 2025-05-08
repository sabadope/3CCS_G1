using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Teacher
{
    public class DeleteStudentModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteStudentModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                // Delete the profile image if it exists
                if (!string.IsNullOrEmpty(student.ImagePath))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, student.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Remove the student from the database
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                // Add success message to TempData
                TempData["SuccessMessage"] = "Student has been successfully deleted.";
                return RedirectToPage("./Students");
            }
            catch (Exception ex)
            {
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the student. Please try again.";
                return Page();
            }
        }
    }
}