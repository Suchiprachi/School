using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("salary_component")]
    public class SalaryComponent
    {
        [Key]
        [Column("component_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComponentId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("component_name")]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("component_type")]
        public string ComponentType { get; set; } = string.Empty; // Fixed | Percent | Calculated

        [Required]
        [Column("is_taxable")]
        public bool IsTaxable { get; set; } = false;

        [Required]
        [Column("is_earning")]
        public bool IsEarning { get; set; } = true;

        [Required]
        [Column("is_deduction")]
        public bool IsDeduction { get; set; } = false;

        // Optional: Navigation property
        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
    }
}
