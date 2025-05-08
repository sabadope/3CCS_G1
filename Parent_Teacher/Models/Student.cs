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

        // Subject 1
        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Class { get; set; } = string.Empty;

        public decimal? Midterm { get; set; }
        public decimal? Finals { get; set; }
        public decimal? TotalAverage { get; set; }

        // Subject 2
        [Required]
        public string Subject2 { get; set; } = string.Empty;

        [Required]
        public string Class2 { get; set; } = string.Empty;

        public decimal? Midterm2 { get; set; }
        public decimal? Finals2 { get; set; }
        public decimal? TotalAverage2 { get; set; }

        // Subject 3
        [Required]
        public string Subject3 { get; set; } = string.Empty;

        [Required]
        public string Class3 { get; set; } = string.Empty;

        public decimal? Midterm3 { get; set; }
        public decimal? Finals3 { get; set; }
        public decimal? TotalAverage3 { get; set; }

        [NotMapped]
        public decimal OverallAverage { get; set; }
    }
}