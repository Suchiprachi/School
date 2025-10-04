namespace Secu_School_API.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
