namespace Secu_School_API.DTOs
{
    public class PublisherDTO
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public int SchoolId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
