using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parent_Teacher.Models
{
    public class CourseSection
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string SectionName { get; set; }
    }
}
