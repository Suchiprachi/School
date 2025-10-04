namespace Secu_School_API.DTOs
{
    public class DepartmentDto
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SchoolId { get; set; }
        public string? SchoolName { get; set; } // For UI display only
    }
}
