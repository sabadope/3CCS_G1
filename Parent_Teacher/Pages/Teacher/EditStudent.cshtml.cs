using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parent_Teacher.Data;
using Parent_Teacher.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Parent_Teacher.Pages.Teacher
{
    public class EditStudentModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditStudentModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Student Student { get; set; }

        public List<SelectListItem> CourseList { get; set; }
        public List<SelectListItem> SubjectList { get; set; }

        public void PopulateCourseList()
        {
            CourseList = _context.CourseSections
                .Select(cs => cs.CourseName)
                .Distinct()
                .Select(c => new SelectListItem { Value = c, Text = c })
                .ToList();
        }

        public void PopulateSubjectList()
        {
            SubjectList = _context.SubjectClasses
                .Select(sc => sc.SubjectName)
                .Distinct()
                .Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                return NotFound();
            }

            PopulateCourseList();
            PopulateSubjectList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> ProfileImage)
        {
            PopulateCourseList();
            PopulateSubjectList();

            var existingStudent = await _context.Students.FindAsync(Student.Id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            // Update basic information
            existingStudent.StudentID = Student.StudentID;
            existingStudent.FirstName = Student.FirstName;
            existingStudent.LastName = Student.LastName;
            existingStudent.Course = Student.Course;
            existingStudent.Section = Student.Section;

            // Update Subject 1
            existingStudent.Subject = Student.Subject;
            existingStudent.Class = Student.Class;
            existingStudent.Midterm = Student.Midterm;
            existingStudent.Finals = Student.Finals;
            existingStudent.TotalAverage = Student.TotalAverage;

            // Update Subject 2
            existingStudent.Subject2 = Student.Subject2;
            existingStudent.Class2 = Student.Class2;
            existingStudent.Midterm2 = Student.Midterm2;
            existingStudent.Finals2 = Student.Finals2;
            existingStudent.TotalAverage2 = Student.TotalAverage2;

            // Update Subject 3
            existingStudent.Subject3 = Student.Subject3;
            existingStudent.Class3 = Student.Class3;
            existingStudent.Midterm3 = Student.Midterm3;
            existingStudent.Finals3 = Student.Finals3;
            existingStudent.TotalAverage3 = Student.TotalAverage3;

            // Handle profile image upload
            if (ProfileImage != null && ProfileImage.Count > 0)
            {
                var file = ProfileImage.First();
                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Path.GetRandomFileName().Substring(0, 8) + Path.GetExtension(file.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingStudent.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingStudent.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingStudent.ImagePath = "/uploads/" + fileName;
            }

            // Calculate Total Averages for all three subjects
            // Subject 1
            if (existingStudent.Midterm != 0 && existingStudent.Finals != 0)
            {
                existingStudent.TotalAverage = (existingStudent.Midterm + existingStudent.Finals) / 2;
            }

            // Subject 2
            if (existingStudent.Midterm2 != 0 && existingStudent.Finals2 != 0)
            {
                existingStudent.TotalAverage2 = (existingStudent.Midterm2 + existingStudent.Finals2) / 2;
            }

            // Subject 3
            if (existingStudent.Midterm3 != 0 && existingStudent.Finals3 != 0)
            {
                existingStudent.TotalAverage3 = (existingStudent.Midterm3 + existingStudent.Finals3) / 2;
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("/Teacher/Students");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        public JsonResult OnGetSections(string course)
        {
            var sections = _context.CourseSections
                .Where(cs => cs.CourseName == course)
                .Select(cs => cs.SectionName)
                .Distinct()
                .ToList();

            return new JsonResult(sections);
        }

        public JsonResult OnGetClasses(string subject)
        {
            var classes = _context.SubjectClasses
                .Where(sc => sc.SubjectName == subject)
                .Select(sc => sc.SubjectCode)
                .Distinct()
                .ToList();

            return new JsonResult(classes);
        }
    }
}