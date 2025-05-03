using System.ComponentModel.DataAnnotations;

namespace Parent_Teacher.Models
{
    public class SubjectClass
    {
        public int Id { get; set; }

        [Required]
        public string SubjectCode { get; set; }

        [Required]
        public string SubjectName { get; set; }  // e.g., Software Engineering 1
    }
}
