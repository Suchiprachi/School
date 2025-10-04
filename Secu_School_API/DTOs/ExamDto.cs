using System.ComponentModel.DataAnnotations;

namespace Secu_School_API.Models.DTOs
{
    public class ExamDto
    {
        public int ExamId { get; set; }

        [Required]
        public int SchoolId { get; set; }
        public string? SchoolName { get; set; }

        [Required]
        public int AcademicYearId { get; set; }
        public string? AcademicYearName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ExamName { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
