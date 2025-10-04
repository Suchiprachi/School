using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("student_enrollment")]
    public class StudentEnrollment
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [Column("gender")]
        public string Gender { get; set; } // Optional: use enum

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Required]
        [Column("section_id")]
        public int SectionId { get; set; }

        [Required]
        [Column("roll_number")]
        public int RollNumber { get; set; }

        [Column("admission_date")]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;

        [Required]
        [Column("status")]
        public string Status { get; set; } = "Active"; // Optional: use enum

        [Column("profile_photo")]
        public string? ProfilePhoto { get; set; }

        [MaxLength(15)]
        [Column("parent_contact")]
        public string? ParentContact { get; set; }

        [MaxLength(100)]
        [Column("parent_email")]
        public string? ParentEmail { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("is_deleted")]
        public int IsDeleted { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }

        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section? Section { get; set; }
    }
}
