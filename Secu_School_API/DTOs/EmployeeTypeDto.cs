namespace Secu_School_API.DTOs
{
    public class EmployeeTypeDto
    {
        public int? EmployeeTypeId { get; set; } // nullable for create

        public string EmployeeTypeName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int SchoolId { get; set; }

        // Optional for frontend display
        public string? SchoolName { get; set; }
    }
}
