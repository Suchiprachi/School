using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using YourNamespace.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public PublisherController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Publisher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDTO>>> GetPublishers()
        {
            try
            {
                var result= await _context.Publishers
                .Select(p => new PublisherDTO
                {
                    PublisherId = p.PublisherId,
                    PublisherName = p.PublisherName,
                    SchoolId = p.SchoolId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToListAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching departments.");
            }
            
        }

        // GET: api/Publisher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDTO>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
                return NotFound();

            return new PublisherDTO
            {
                PublisherId = publisher.PublisherId,
                PublisherName = publisher.PublisherName,
                SchoolId = publisher.SchoolId,
                CreatedAt = publisher.CreatedAt,
                UpdatedAt = publisher.UpdatedAt
            };
        }

        // POST: api/Publisher
        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] PublisherDTO dto)
        {
            var publisher = new Publisher
            {
                PublisherName = dto.PublisherName,
                SchoolId = dto.SchoolId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Publisher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] PublisherDTO dto)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
                return NotFound();

            publisher.PublisherName = dto.PublisherName;
            publisher.SchoolId = dto.SchoolId;
            publisher.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Publisher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
                return NotFound();

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
