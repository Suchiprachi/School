namespace Secu_School_API.DTOs
{
    public class BookCopyDto
    {
        public int CopyId { get; set; }

        public int BookId { get; set; }

        public string Barcode { get; set; } = string.Empty;

        public int? LocationId { get; set; }

        public string Status { get; set; } = "available"; // available, issued, damaged, etc.

        public int IsDeleted { get; set; }

        public int? IssuedTo { get; set; }

        public int? RoleId { get; set; }

        public DateTime? IssuedOn { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
