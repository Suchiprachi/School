using System;

namespace Secu_School_API.DTOs
{
    public class AcademicYearDto
    {
        public int AcademicYearId { get; set; }
        public string? YearName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public int SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public int IsDeleted { get; set; }

    }
}


