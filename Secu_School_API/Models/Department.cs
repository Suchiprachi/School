using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("department")]
    public class Department
    {
        [Key]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("department_name")]
        public string DepartmentName { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }

        // Optional: Navigation to Designations
        public virtual ICollection<Designation>? Designations { get; set; }
        public virtual ICollection<Staff>? Staffs { get; set; }
    }
}
