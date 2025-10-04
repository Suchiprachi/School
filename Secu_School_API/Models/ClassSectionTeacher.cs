using Secu_School_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ClassSectionTeacher
{
    [Column("id")]
    public int Id { get; set; }
    [Column("class_id")]
    public int ClassId { get; set; }
    [Column("section_id")] 
    public int SectionId { get; set; }
    [Column("staff_id")]
    public int StaffId { get; set; }
    [Column("subject_id")]
    public int SubjectId { get; set; }
    [Column("school_id")]
    public int SchoolId { get; set; }

    [Column("academic_year_id")]
    public int AcademicYearId { get; set; }
    [Column("is_deleted")]
    public int IsDeleted { get; set; }

    // Navigation properties
    public virtual Class? Class { get; set; }
    public virtual Section? Section { get; set; }
    public virtual Staff? Staff { get; set; }
    public virtual Subject? Subject { get; set; }
    public virtual School? School { get; set; }
    public virtual AcademicYear? AcademicYear { get; set; }
}
