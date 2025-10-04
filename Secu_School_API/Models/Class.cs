using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("class")] // Optional, but useful if table name is a reserved keyword or case-sensitive in some DBs
    public class Class
    {
        [Key]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("class_name")]
        public string ClassName { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("is_deleted")]
        public int IsDeleted { get; set; } = 0; 

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("SchoolId")]
        public School? School { get; set; }

        public ICollection<StudentEnrollment>? StudentEnrollments { get; set; }
        public ICollection<ClassSectionTeacher>? ClassSectionTeachers { get; set; }
        public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();
    }
}
