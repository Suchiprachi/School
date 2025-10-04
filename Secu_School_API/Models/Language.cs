using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("language")]
    public class Language
    {
        [Key]
        [Column("language_id")]
        public int LanguageId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("language_name")]
        public string LanguageName { get; set; } = string.Empty;

        [MaxLength(10)]
        [Column("language_code")]
        public string? LanguageCode { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }
    }
}