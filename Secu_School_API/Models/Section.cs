using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("section")]
    public class Section
    {
        [Key]
        [Column("section_id")]
        public int SectionId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("section_name")]
        public string SectionName { get; set; } = string.Empty;

        [Required]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("is_deleted")]
        public int IsDeleted { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
        public virtual ICollection<ClassSectionTeacher>? ClassSectionTeachers { get; set; }
        public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();
    }
}
