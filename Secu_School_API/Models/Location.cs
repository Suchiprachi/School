using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("location")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("location_name")]
        public string? LocationName { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }

        public virtual ICollection<BookCopy>? BookCopies { get; set; }



    }
}