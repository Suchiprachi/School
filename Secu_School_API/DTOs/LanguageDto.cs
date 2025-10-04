using System.ComponentModel.DataAnnotations;

namespace Secu_School_API.DTOs
{
    public class LanguageDto
    {
        public int? LanguageId { get; set; }

        [Required]
        [StringLength(100)]
        public string LanguageName { get; set; } = string.Empty;

        [StringLength(10)]
        public string? LanguageCode { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public int SchoolId { get; set; }

        public string? SchoolName { get; set; }  // For display only
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}