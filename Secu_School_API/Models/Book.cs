using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourNamespace.Models;

namespace Secu_School_API.Models
{
    [Table("book")]
    public class Book
    {
        [Key]
        [Column("book_id")]
        public int BookId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("isbn")]
        public string? ISBN { get; set; }

        [Column("author_id")]
        public int? AuthorId { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [Column("publisher_id")]
        public int? PublisherId { get; set; }

        [Column("year_published")]
        public int? YearPublished { get; set; }

        [Required]
        [Column("total_copies")]
        public int TotalCopies { get; set; }

        [Required]
        [Column("available_copies")]
        public int AvailableCopies { get; set; }

        [Required]
        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("edition")]
        [MaxLength(50)]
        public string? Edition { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("num_pages")]
        public int? NumberOfPages { get; set; }

        [Column("status")]
        [MaxLength(20)]
        public string Status { get; set; } = "active";

        [Column("is_deleted")]
        public int IsDeleted { get; set; }

        [Column("language_id")]
        public int? LanguageId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(AuthorId))]
        public virtual Author? Author { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [ForeignKey(nameof(PublisherId))]
        public virtual Publisher? Publisher { get; set; }

        [ForeignKey(nameof(SchoolId))]
        public virtual School? School { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public virtual Language? Language { get; set; }

        // Navigation property to BookCopies (optional)
        public virtual ICollection<BookCopy>? Copies { get; set; }
    }
}
