using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("designation")]
    public class Designation
    {
        [Key]
        [Column("designation_id")]
        public int DesignationId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("designation_name")]
        public string DesignationName { get; set; } = string.Empty;

        [Required]
        [Column("department_id")]
        public int DepartmentId { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
        public virtual Staff? Staffs { get; set; }
    }
}
