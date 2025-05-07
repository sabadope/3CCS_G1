using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Parent_Teacher.Models;
using Parent_Teacher.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Parent_Teacher.Pages.Teacher
{
    public class ViewGradeModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        public ViewGradeModel(IWebHostEnvironment environment,
                                AppDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public Grade Grade { get; set; } = new(); // For adding new grade

        public IFormFile? StudentPhoto { get; set; }

        public string StudentImageUrl { get; set; } = "/images/default.png";

        public Student Student { get; set; } = default!;
        public List<Grade> Grades { get; set; } = new(); // To hold grades

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Get student data
            Student = await _context.Students.FindAsync(id);
            if (Student == null)
                return NotFound();

            // Get grades of the student
            Grades = await _context.Grades
                .Where(g => g.StudentId == id)
                .ToListAsync();


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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Add the new grade
            if (!ModelState.IsValid)
                return Page();

            Grade.StudentId = id;
            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            // Redirect to the same page after adding the grade
            return RedirectToPage(new { id });
        }


        public async Task<IActionResult> OnPostUploadStudentPhotoAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null || StudentPhoto == null || StudentPhoto.Length == 0)
                return RedirectToPage(new { id });

            var ext = Path.GetExtension(StudentPhoto.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif")
            {
                TempData["Error"] = "Only JPG, PNG, or GIF files are allowed.";
                return RedirectToPage(new { id });
            }

            if (StudentPhoto.Length > 2 * 1024 * 1024)
            {
                TempData["Error"] = "Max file size is 2MB.";
                return RedirectToPage(new { id });
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            // Optional: remove old photo
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                var oldPath = Path.Combine(uploadsFolder, student.ImagePath);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var uniqueFileName = Guid.NewGuid() + ext;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await StudentPhoto.CopyToAsync(stream);
            }

            student.ImagePath = uniqueFileName;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }
    }
}
