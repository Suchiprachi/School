using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourNamespace.Models;

namespace Secu_School_API.Models
{
    [Table("school")] // Optional but recommended
    public class School
    {
        [Key]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("school_name")]
        public string SchoolName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("principal_name")]
        public string PrincipalName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("school_logo")]
        public string SchoolLogo { get; set; }

        [Required]
        [MaxLength(8)]
        [Column("status")]
        public string Status { get; set; } = "Active"; // Consider using Enum

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Optional Navigation Properties
        public ICollection<StudentEnrollment>? StudentEnrollments { get; set; }
        public ICollection<AcademicYear>? AcademicYears { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Designation>? Designations { get; set; }
        public ICollection<SalaryComponent>? SalaryComponents { get; set; }
        public ICollection<Staff>? Staffs { get; set; }
        public ICollection<ClassSectionTeacher>? ClassSectionTeachers { get; set; }
        public ICollection<FineMaster>? FineMasters { get; set; }
        public ICollection<Author>? Authors { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Publisher>? Publishers { get; set; }
        public ICollection<Location>? Locations { get; set; }
        public ICollection<Language>? Languages { get; set; }
        public ICollection<Book>? Books { get; set; }
        public ICollection<Role>? Roles { get; set; }
        public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        
    }
}
