using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parent_Teacher.Models
{
    [Table("Students")]
    public class Student
    {
        public int Id { get; set; }

        public string StudentID { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Course { get; set; } = string.Empty;

        [Required]
        public string Section { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? ImagePath { get; set; }

        // Add this property
        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Class { get; set; } = string.Empty;

        // Change to nullable decimal
        public decimal? Midterm { get; set; }
        public decimal? Finals { get; set; }

        // Calculate Total Average
        public decimal? TotalAverage { get; set; }


    }
}
