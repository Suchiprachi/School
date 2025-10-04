namespace Secu_School_API.DTOs
{
    public class LeaveTypeDto
    {
        public int? LeaveTypeId { get; set; }

        public int SchoolId { get; set; }

        public string LeaveTypeName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsPaid { get; set; } = true;

        public int MaxDaysPerYear { get; set; } = 0;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
