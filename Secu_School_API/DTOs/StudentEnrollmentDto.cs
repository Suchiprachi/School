using Microsoft.AspNetCore.Http;

namespace Secu_School_API.DTOs
{
    public class StudentEnrollmentDto
    {
        public int? StudentId { get; set; } // Nullable for create

        public int SchoolId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = "Male"; // or use enum

        public DateTime DateOfBirth { get; set; }

        public int ClassId { get; set; }

        public int SectionId { get; set; }

        public int RollNumber { get; set; }

        public DateTime? AdmissionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Active"; // or use enum

        public string? ParentContact { get; set; }

        public string? ParentEmail { get; set; }

        public string? Address { get; set; }

        public int? IsDeleted { get; set; } = 0;

        public IFormFile? ProfilePhoto { get; set; } // for file uploads
    }
}
