using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Parent_Teacher.Pages.Teacher
{
    public class ViewStudentModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        public ViewStudentModel(IWebHostEnvironment environment,
                                AppDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public IFormFile? StudentPhoto { get; set; }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public string StudentImageUrl { get; set; } = "/images/default.png";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);
            if (Student == null)
                return NotFound();

            // If a custom image exists, use it
            if (!string.IsNullOrEmpty(Student.ImagePath))
            {
                var path = Path.Combine(_environment.WebRootPath,
                                        "uploads",
                                        Student.ImagePath);
                if (System.IO.File.Exists(path))
                    StudentImageUrl = "/uploads/" + Student.ImagePath;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadStudentPhotoAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null || StudentPhoto == null || StudentPhoto.Length == 0)
                return RedirectToPage(new { id });

            // Validate file type & size (optional, but recommended)
            var ext = Path.GetExtension(StudentPhoto.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
            {
                ModelState.AddModelError(string.Empty,
                    "Only JPG, PNG or GIF files are allowed.");
                return await OnGetAsync(id);
            }
            if (StudentPhoto.Length > 2 * 1024 * 1024) // 2 MB
            {
                ModelState.AddModelError(string.Empty,
                    "Max file size is 2 MB.");
                return await OnGetAsync(id);
            }

            // Ensure uploads folder exists
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            // Delete old file (optional cleanup)
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                var old = Path.Combine(uploadsFolder, student.ImagePath);
                if (System.IO.File.Exists(old))
                    System.IO.File.Delete(old);
            }

            // Save new file
            var uniqueFileName = Guid.NewGuid().ToString()
                                 + Path.GetExtension(StudentPhoto.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await StudentPhoto.CopyToAsync(stream);

            // Update DB
            student.ImagePath = uniqueFileName;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
