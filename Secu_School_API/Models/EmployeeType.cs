using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("employee_type")]
    public class EmployeeType
    {
        [Key]
        [Column("employee_type_id")]
        public int EmployeeTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("employee_type_name")]
        public string EmployeeTypeName { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
    }
}
