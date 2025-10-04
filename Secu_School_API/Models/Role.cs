using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("roles")] // Optional: if your table is named "roles"
    public class Role
    {
        [Key]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("role_name")]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        // 🔗 Navigation property to SchoolMaster
        [ForeignKey("SchoolId")]
        public School? School { get; set; }

        // 🔁 Reverse navigation from FineMaster (1 Role → many Fines)
        public ICollection<FineMaster>? Fines { get; set; }

        public ICollection<Staff>? Staffs { get; set; }
    }
}
