using System.ComponentModel.DataAnnotations;

namespace Secu_School_API.DTOs
{
    public class LocationDto
    {
        public int? LocationId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Location name cannot exceed 100 characters")]
        public string LocationName { get; set; } = string.Empty;

        [Required]
        public int SchoolId { get; set; }

        public string? SchoolName { get; set; }  // For UI display only

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}