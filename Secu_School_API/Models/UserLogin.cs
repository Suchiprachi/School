using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("user_login")]
    public class UserLogin
    {
        [Key]
        [Column("login_id")]
        public int LoginId { get; set; }

        [Required]
        [Column("ref_id")]
        public int RefId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("user_type")]
        public string UserType { get; set; } = string.Empty;  // e.g., "Admin", "Teacher"

        [Required]
        [MaxLength(100)]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; } = "Active";  // "Active" or "Inactive"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
    }
}
