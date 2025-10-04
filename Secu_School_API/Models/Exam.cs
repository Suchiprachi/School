using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("exams")]
    public class Exam
    {
        [Key]
        [Column("exam_id")]
        public int ExamId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [Column("academic_year_id")]
        public int AcademicYearId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("exam_name")]
        public string ExamName { get; set; }

        [MaxLength(255)]
        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // 🔗 Relationships
       
        public School? School { get; set; }

        
        public AcademicYear? AcademicYear { get; set; }

      
        public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();
    }
}
