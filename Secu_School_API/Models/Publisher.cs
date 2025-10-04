using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Secu_School_API.Models;

namespace YourNamespace.Models  // Replace with your actual namespace
{
    public class Publisher
    {
        [Key]
        [Column("publisher_id")]
        public int PublisherId { get; set; }

        [Required]
        [Column("publisher_name")]
        [StringLength(255)]
        public string PublisherName { get; set; } = string.Empty;

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; } = 1; // hardcoded for school_id = 1

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public School? School { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
