namespace Secu_School_API.DTOs
{
    public class FineMasterDTO
    {
        public int FineId { get; set; }
        public int SchoolId { get; set; } = 1;
        public int RoleId { get; set; }
        public decimal FinePerDay { get; set; }
        public int GracePeriod { get; set; } = 0;
        public decimal? MaxFine { get; set; }
        public string Status { get; set; } = "active";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Optional: include Role Name if needed in UI
        public string? RoleName { get; set; }
    }
}
