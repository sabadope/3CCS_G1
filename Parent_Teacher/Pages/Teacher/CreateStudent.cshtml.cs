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
    public class CreateStudentModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateStudentModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
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

        public IActionResult OnGet()
        {
            PopulateCourseList();
            PopulateSubjectList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> ProfileImage)
        {
            PopulateCourseList();
            PopulateSubjectList();

            

            // Handling the profile image upload
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

                Student.ImagePath = "/uploads/" + fileName;
            }

            // Calculate Total Averages for all three subjects
            // Subject 1
            if (Student.Midterm != 0 && Student.Finals != 0)
            {
                Student.TotalAverage = (Student.Midterm + Student.Finals) / 2;
            }

            // Subject 2
            if (Student.Midterm2 != 0 && Student.Finals2 != 0)
            {
                Student.TotalAverage2 = (Student.Midterm2 + Student.Finals2) / 2;
            }

            // Subject 3
            if (Student.Midterm3 != 0 && Student.Finals3 != 0)
            {
                Student.TotalAverage3 = (Student.Midterm3 + Student.Finals3) / 2;
            }

            // Save the student record
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Teacher/Students");
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