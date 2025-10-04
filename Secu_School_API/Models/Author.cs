using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("author")]
    public class Author
    {
        [Key]
        [Column("author_id")]
        public int AuthorId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // ✅ Add this navigation property
        [ForeignKey("SchoolId")]
        public virtual School? School { get; set; }
    }
}
