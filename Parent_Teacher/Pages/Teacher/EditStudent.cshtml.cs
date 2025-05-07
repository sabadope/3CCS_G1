using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.ComponentModel.DataAnnotations;

namespace Parent_Teacher.Pages.Teacher
{
    public class EditStudentModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditStudentModel(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Student Student { get; set; } = new();

        [BindProperty]
        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImage { get; set; }

        public List<SelectListItem> CourseList { get; set; } = new();
        public List<SelectListItem> SubjectList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Student? student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            Student = student;

            LoadSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadSelectLists();
                return Page();
            }

            var studentInDb = await _context.Students.FindAsync(Student.Id);
            if (studentInDb == null) return NotFound();

            // Update properties
            studentInDb.StudentID = Student.StudentID;
            studentInDb.FirstName = Student.FirstName;
            studentInDb.LastName = Student.LastName;
            studentInDb.Course = Student.Course;
            studentInDb.Section = Student.Section;
            studentInDb.Subject = Student.Subject;
            studentInDb.Class = Student.Class;
            studentInDb.Midterm = Student.Midterm;
            studentInDb.Finals = Student.Finals;
            studentInDb.TotalAverage = (Student.Midterm + Student.Finals) / 2;

            // Handle Profile Image upload
            if (ProfileImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                studentInDb.ImagePath = "/uploads/" + uniqueFileName; // FIX: Match DB column
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Teacher/Students");
        }

        public IActionResult OnGetSections(string course)
        {
            var sections = _context.CourseSections
                .Where(cs => cs.CourseName == course)
                .Select(cs => cs.SectionName)
                .Distinct()
                .ToList();

            return new JsonResult(sections);
        }

        public IActionResult OnGetClasses(string subject)
        {
            var classes = _context.SubjectClasses
                .Where(sc => sc.SubjectName == subject)
                .Select(sc => sc.SubjectCode)
                .Distinct()
                .ToList();

            return new JsonResult(classes);
        }

        private void LoadSelectLists()
        {
            CourseList = _context.CourseSections
                .Select(c => new SelectListItem
                {
                    Value = c.CourseName,
                    Text = c.CourseName
                })
                .Distinct()
                .ToList();

            SubjectList = _context.SubjectClasses
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectName,
                    Text = s.SubjectName
                })
                .Distinct()
                .ToList();
        }
    }
}
