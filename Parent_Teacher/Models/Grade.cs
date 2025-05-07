using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parent_Teacher.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }

        public int GradeValue { get; set; }  // Renamed from Grade to GradeValue
        public string SubjectName { get; set; } = string.Empty;

        public Student Student { get; set; }
    }
}
