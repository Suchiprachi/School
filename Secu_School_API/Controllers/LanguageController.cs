using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public LanguageController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Language
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageDto>>> GetLanguages()
        {
            try
            {
                var languages = await _context.Languages
                    .Include(l => l.School)
                    .Select(l => new LanguageDto
                    {
                        LanguageId = l.LanguageId,
                        LanguageName = l.LanguageName,
                        LanguageCode = l.LanguageCode,
                        IsActive = l.IsActive,
                        SchoolId = l.SchoolId,
                        SchoolName = l.School!.SchoolName,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new { data = languages });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching languages.");
            }
        }

        // GET: api/Language/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageDto>> GetLanguage(int id)
        {
            try
            {
                var language = await _context.Languages
                    .Include(l => l.School)
                    .Where(l => l.LanguageId == id)
                    .Select(l => new LanguageDto
                    {
                        LanguageId = l.LanguageId,
                        LanguageName = l.LanguageName,
                        LanguageCode = l.LanguageCode,
                        IsActive = l.IsActive,
                        SchoolId = l.SchoolId,
                        SchoolName = l.School!.SchoolName,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                return language == null ? NotFound() : Ok(language);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the language.");
            }
        }

        // POST: api/Language
        [HttpPost]
        public async Task<ActionResult<Language>> CreateLanguage(LanguageDto languageDto)
        {
            try
            {
                var language = new Language
                {
                    LanguageName = languageDto.LanguageName,
                    LanguageCode = languageDto.LanguageCode,
                    IsActive = languageDto.IsActive,
                    SchoolId = languageDto.SchoolId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Languages.Add(language);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLanguage), new { id = language.LanguageId }, language);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    error = "Database error. Possible duplicate language name.",
                    details = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while creating the language.",
                    details = ex.Message
                });
            }
        }

        // PUT: api/Language/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLanguage(int id, LanguageDto languageDto)
        {
            if (id != languageDto.LanguageId)
                return BadRequest("Language ID mismatch");

            var language = await _context.Languages.FindAsync(id);
            if (language == null)
                return NotFound();

            try
            {
                language.LanguageName = languageDto.LanguageName;
                language.LanguageCode = languageDto.LanguageCode;
                language.IsActive = languageDto.IsActive;
                language.SchoolId = languageDto.SchoolId;
                language.UpdatedAt = DateTime.UtcNow;

                _context.Entry(language).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id)) return NotFound();
                throw;
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    error = "Database error. Possible duplicate language name.",
                    details = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while updating the language.",
                    details = ex.Message
                });
            }
        }

        // DELETE: api/Language/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            try
            {
                var language = await _context.Languages.FindAsync(id);
                if (language == null) return NotFound();

                _context.Languages.Remove(language);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while deleting the language.",
                    details = ex.Message
                });
            }
        }

        private bool LanguageExists(int id)
        {
            return _context.Languages.Any(e => e.LanguageId == id);
        }
    }
}