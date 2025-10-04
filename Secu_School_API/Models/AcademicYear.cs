using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Secu_School_API.Models
{
    [Table("academic_year")]
    public class AcademicYear
    {
        [Key]
        [Column("academic_year_id")]
        public int AcademicYearId { get; set; }

        [Column("year_name")]
        [MaxLength(20)]
        public string YearName { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("is_current")]
        public bool IsCurrent { get; set; }

        [Column("school_id")]
        public int SchoolId { get; set; }

        [Column("is_deleted")]
        public int IsDeleted { get; set; }

        // Navigation property (if you have School model)
        public School? School { get; set; }
        public ICollection<Exam> Exams { get; set; }

        public virtual ICollection<ClassSectionTeacher>? ClassSectionTeachers { get; set; }
        public virtual ICollection<StudentEnrollment>? StudentEnrollments { get; set; }
    }
}
