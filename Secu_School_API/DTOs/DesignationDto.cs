namespace Secu_School_API.DTOs
{
    public class DesignationDto
    {
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string? Description { get; set; }
        public int SchoolId { get; set; }

        // Optional for UI display
        public string? DepartmentName { get; set; }
        public string? SchoolName { get; set; }
    }
}
