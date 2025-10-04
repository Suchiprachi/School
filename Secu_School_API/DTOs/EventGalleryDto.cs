using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Secu_School_API.DTOs
{
    public class EventGalleryDto
    {
        public int EventGalleryId { get; set; }
        public int SchoolId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? MediaType { get; set; }

        // Multiple files
        public List<IFormFile>? Files { get; set; }
    }
}
