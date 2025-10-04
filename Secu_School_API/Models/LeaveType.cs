using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("leave_type")]
    public class LeaveType
    {
        [Key]
        [Column("leave_type_id")]
        public int LeaveTypeId { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("leave_type_name")]
        public string LeaveTypeName { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_paid")]
        public bool IsPaid { get; set; } = true;

        [Column("max_days_per_year")]
        public int MaxDaysPerYear { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
    }
}
