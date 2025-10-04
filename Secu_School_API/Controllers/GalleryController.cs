using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly SchoolDbContext _context;

        public GalleryController(SchoolDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Create Gallery (single or multiple files)
        [HttpPost("create-gallery")]
        public async Task<IActionResult> CreateGallery([FromForm] GalleryDto dto)
        {
            if (dto.Files == null || !dto.Files.Any())
                return BadRequest("At least one file is required.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
            var galleries = new List<Gallery>();

            foreach (var file in dto.Files)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("Invalid file type: " + file.FileName);

                string filePath = await SaveFile(file);

                galleries.Add(new Gallery
                {
                    SchoolId = dto.SchoolId,
                    Title = dto.Title,
                    Description = dto.Description ?? "",
                    FileName = file.FileName,
                    FilePath = filePath,
                    MediaType = ext == ".mp4" ? "video" : "image",
                    UploadedAt = DateTime.Now
                });
            }

            _context.Galleries.AddRange(galleries);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Gallery created successfully", galleries });
        }

        // ✅ Update gallery
        [HttpPut("update-gallery/{id}")]
        public async Task<IActionResult> UpdateGallery(int id, [FromForm] GalleryDto dto)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return NotFound("Gallery not found.");

            gallery.Title = dto.Title;
            gallery.Description = dto.Description ?? gallery.Description;
            gallery.MediaType = dto.MediaType ?? gallery.MediaType;

            if (dto.File != null)
            {
                // Delete old file
                var oldFilePath = Path.Combine(_env.WebRootPath, gallery.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
                var ext = Path.GetExtension(dto.File.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("Invalid file type.");

                string filePath = await SaveFile(dto.File);
                gallery.FileName = dto.File.FileName;
                gallery.FilePath = filePath;
                gallery.MediaType = ext == ".mp4" ? "video" : "image";
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Gallery updated successfully", gallery });
        }

        // ✅ Get all galleries by school
        [HttpGet("get-gallery/{schoolId}")]
        public async Task<IActionResult> GetAllGallery(int schoolId)
        {
            var galleries = await _context.Galleries
                .Where(g => g.SchoolId == schoolId)
                .OrderByDescending(g => g.UploadedAt)
                .ToListAsync();

            return Ok(galleries);
        }

        // ✅ Get gallery by ID
        [HttpGet("get-gallery-by-id/{id}")]
        public async Task<IActionResult> GetGalleryById(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return NotFound();
            return Ok(gallery);
        }

        // ✅ Delete gallery
        [HttpDelete("delete-gallery/{id}")]
        public async Task<IActionResult> DeleteGallery(int id)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return NotFound("Gallery not found.");

            string filePath = Path.Combine(_env.WebRootPath, gallery.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString()).TrimStart(Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Gallery deleted successfully" });
        }

        // 🔹 Helper: Save file
        private async Task<string> SaveFile(IFormFile file)
        {
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", dateFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{dateFolder}/{fileName}";
        }

        // ✅ Serve file
        [HttpGet("file/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
            if (!System.IO.File.Exists(path)) return NotFound();

            var mime = "application/octet-stream";
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out mime);
            return PhysicalFile(path, mime ?? "application/octet-stream");
        }
    }
}
