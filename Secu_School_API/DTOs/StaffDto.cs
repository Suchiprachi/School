namespace Secu_School_API.DTOs
{
    public class StaffDto
    {
        public int? StaffId { get; set; }  // Nullable for Create operations (auto-generated)

        public int SchoolId { get; set; }

        public string? Name { get; set; }

        public int RoleId { get; set; }

        public string? Gender { get; set; }  // Should be "Male", "Female", or "Other"

        public DateTime? DateOfBirth { get; set; }

        public DateTime? JoiningDate { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? DepartmentId { get; set; }

        public int? DesignationId { get; set; }

        public int? EmployeeTypeId { get; set; }

        public int IsActive { get; set; }

        public int IsDeleted { get; set; }

        public string? ProfilePhoto { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
