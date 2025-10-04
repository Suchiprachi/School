using System.ComponentModel.DataAnnotations;

namespace Secu_School_API.Dtos
{
    public class ClassSectionTeacherDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int SectionId { get; set; }
        public int StaffId { get; set; }
        public int SchoolId { get; set; }
        public int AcademicYearId { get; set; }

        public int IsDeleted { get; set; }

        public string? ClassName { get; set; }
        public string? SubjectName { get; set; }
        public string? SectionName { get; set; }
        public string? TeacherName { get; set; }
        public string? SchoolName { get; set; }
        public string? AcademicYearName { get; set; }
    }
}