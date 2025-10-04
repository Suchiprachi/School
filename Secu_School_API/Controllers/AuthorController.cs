using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;
using Secu_School_API.DTOs;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public AuthorController(SchoolDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get() => await _context.Authors.ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Author>> Post(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = author.AuthorId }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Author author)
        {
            if (id != author.AuthorId) return BadRequest();
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}