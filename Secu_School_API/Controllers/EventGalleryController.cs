using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGalleryController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EventGalleryController(SchoolDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Create Event Gallery with multiple files
        [HttpPost("create-event-gallery")]
        public async Task<IActionResult> CreateEventGallery([FromForm] EventGalleryDto dto)
        {
            if (dto.Files == null || !dto.Files.Any())
                return BadRequest("At least one file is required.");

            var eventGallery = new EventGallery
            {
                SchoolId = dto.SchoolId,
                Title = dto.Title,
                Description = dto.Description ?? "",
                MediaType = dto.MediaType ?? "image",
                CreatedAt = DateTime.Now
               
            };

            _context.EventGalleries.Add(eventGallery);
            await _context.SaveChangesAsync();

            foreach (var file in dto.Files)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                    return BadRequest("Invalid file type: " + file.FileName);

                string filePath = await SaveFile(file);

                var galleryFile = new GalleryFile
                {
                    EventGalleryId = eventGallery.EventGalleryId,
                    FileName = file.FileName,
                    FilePath = filePath,
                    MediaType = ext == ".mp4" ? "video" : "image",
                    UploadedAt = DateTime.Now
                };

                _context.GalleryFiles.Add(galleryFile);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Event gallery created successfully", eventGalleryId = eventGallery.EventGalleryId });
        }

        // ✅ Get all Event Galleries by school (with files)
        [HttpGet("get-event-galleries/{schoolId}")]
        public async Task<IActionResult> GetEventGalleries(int schoolId)
        {
            var galleries = await _context.EventGalleries
                .Include(e => e.Files)
                .Where(e => e.SchoolId == schoolId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return Ok(galleries);
        }

        // ✅ Get single Event Gallery by ID (with files)
        [HttpGet("get-event-gallery/{id}")]
        public async Task<IActionResult> GetEventGalleryById(int id)
        {
            var gallery = await _context.EventGalleries
                .Include(e => e.Files)
                .FirstOrDefaultAsync(e => e.EventGalleryId == id);

            if (gallery == null) return NotFound("Event gallery not found.");
            return Ok(gallery);
        }

        // ✅ Update Event Gallery (title, description, media type)
        [HttpPut("update-event-gallery/{id}")]
        public async Task<IActionResult> UpdateEventGallery(int id, [FromForm] EventGalleryDto dto)
        {
            var gallery = await _context.EventGalleries.FindAsync(id);
            if (gallery == null) return NotFound("Event gallery not found.");

            gallery.Title = dto.Title;
            gallery.Description = dto.Description ?? gallery.Description;
            gallery.MediaType = dto.MediaType ?? gallery.MediaType;

            // Optional: Add new files
            if (dto.Files != null && dto.Files.Any())
            {
                foreach (var file in dto.Files)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
                    var ext = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(ext))
                        return BadRequest("Invalid file type: " + file.FileName);

                    string filePath = await SaveFile(file);

                    _context.GalleryFiles.Add(new GalleryFile
                    {
                        EventGalleryId = gallery.EventGalleryId,
                        FileName = file.FileName,
                        FilePath = filePath,
                        MediaType = ext == ".mp4" ? "video" : "image",
                        UploadedAt = DateTime.Now
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Event gallery updated successfully" });
        }

        // ✅ Delete Event Gallery + cascade delete files (DB + disk)
        [HttpDelete("delete-event-gallery/{id}")]
        public async Task<IActionResult> DeleteEventGallery(int id)
        {
            var gallery = await _context.EventGalleries
                .Include(e => e.Files)
                .FirstOrDefaultAsync(e => e.EventGalleryId == id);

            if (gallery == null) return NotFound("Event gallery not found.");

            foreach (var file in gallery.Files)
            {
                var filePath = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.EventGalleries.Remove(gallery);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event gallery and files deleted successfully" });
        }

        // 🔹 Helper: Save file
        private async Task<string> SaveFile(IFormFile file)
        {
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string uploadsFolder = Path.Combine(_env.WebRootPath, "event-gallery", dateFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/event-gallery/{dateFolder}/{fileName}";
        }
    }
}
