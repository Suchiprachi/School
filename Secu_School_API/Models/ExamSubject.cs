using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("exam_subjects")]
    public class ExamSubject
    {
        [Key]
        [Column("exam_subject_id")]
        public int ExamSubjectId { get; set; }

        [Required]
        [Column("exam_id")]
        public int ExamId { get; set; }

        [Required]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Column("section_id")]
        public int? SectionId { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Column("staff_id")]
        public int? StaffId { get; set; }

        [Required]
        [Column("exam_date")]
        public DateTime ExamDate { get; set; }

        [Required]
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Column("max_marks", TypeName = "decimal(5,2)")]
        public decimal MaxMarks { get; set; }

        [Required]
        [Column("passing_marks", TypeName = "decimal(5,2)")]
        public decimal PassingMarks { get; set; }

        // 🔗 Relationships (optional, good for EF navigation)
        [ForeignKey("ExamId")]
        public Exam? Exam { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }

        [ForeignKey("SectionId")]
        public Section? Section { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }
        

    }
}
