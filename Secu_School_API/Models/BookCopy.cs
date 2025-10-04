using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("book_copies")]
    public class BookCopy
    {
        [Key]
        [Column("copy_id")]
        public int CopyId { get; set; }

        [Required]
        [Column("book_id")]
        public int BookId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("barcode")]
        public string Barcode { get; set; } = string.Empty;

        [Column("location_id")]
        public int? LocationId { get; set; }

        [MaxLength(20)]
        [Column("status")]
        public string Status { get; set; } = "available"; // available, issued, damaged, etc.

        [Column("is_deleted")]
        public int IsDeleted { get; set; } 

        [Column("issued_to")]
        public int? IssuedTo { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        [Column("issued_on")]
        public DateTime? IssuedOn { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(BookId))]
        public virtual Book? Book { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }


        }
}
