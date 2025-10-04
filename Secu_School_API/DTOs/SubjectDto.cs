using Microsoft.AspNetCore.Http;

namespace Secu_School_API.DTOs
{
    public class SubjectDto
    {
        public int? SubjectId { get; set; } // Nullable for create
        public string SubjectName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public int SchoolId { get; set; }
    }
}
