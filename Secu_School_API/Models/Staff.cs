
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("staff")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        public int StaffId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("gender")]
        public string Gender { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("joining_date")]
        public DateTime? JoiningDate { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(15)]
        [Column("phone")]
        public string? Phone { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("department_id")]
        public int? DepartmentId { get; set; }

        [Column("designation_id")]
        public int? DesignationId { get; set; }

        [Column("employee_type_id")]
        public int? EmployeeTypeId { get; set; }

        [Required]
        [Column("is_active")]
        public int IsActive { get; set; }

        [Column("is_deleted")]
        public int IsDeleted { get; set; }

        [Column("profile_photo")]
        public int? ProfilePhoto { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual School? School { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Designation? Designation { get; set; }
        public virtual EmployeeType? EmployeeType { get; set; }
        public virtual ICollection<ClassSectionTeacher>? ClassSectionTeachers { get; set; }
        public virtual ICollection<ExamSubject> ExamSubjects { get; set; } = new List<ExamSubject>();
    }
}
