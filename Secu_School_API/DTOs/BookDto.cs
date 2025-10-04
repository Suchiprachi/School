using System.ComponentModel.DataAnnotations;

namespace Secu_School_API.DTOs
{
    public class BookDto
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ISBN { get; set; }

        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }
        public int? YearPublished { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }

        [Required]
        public int SchoolId { get; set; }
        public string? Edition { get; set; }
        public string? Description { get; set; }
        public int? NumberOfPages { get; set; }
        public string Status { get; set; } = "active";
        public int IsDeleted { get; set; }
        public int? LanguageId { get; set; }

    }
}
