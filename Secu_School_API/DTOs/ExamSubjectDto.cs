namespace Secu_School_API.DTOs
{
    public class ExamSubjectDto
    {
        public int ExamId { get; set; }
        public int ClassId { get; set; }
        public int? SectionId { get; set; }
        public int SubjectId { get; set; }
        public int? StaffId { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal MaxMarks { get; set; }
        public decimal PassingMarks { get; set; }
    }
}
