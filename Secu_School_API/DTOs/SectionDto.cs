namespace Secu_School_API.Dtos
{
    public class SectionDto
    {
        public int? SectionId { get; set; } // Nullable for create
        public string SectionName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public int SchoolId { get; set; }
    }
}
