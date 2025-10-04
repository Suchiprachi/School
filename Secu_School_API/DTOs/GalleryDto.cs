using Microsoft.AspNetCore.Http;
using System;

namespace Secu_School_API.DTOs
{
    public class GalleryDto
    {
        public int GalleryId { get; set; }
        public int SchoolId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? MediaType { get; set; }
        public DateTime UploadedAt { get; set; }
        public IFormFile? File { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
