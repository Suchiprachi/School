using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;
using Secu_School_API.DTOs;



namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SubjectController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetSubjects")]
        public async Task<IActionResult> GetSubjects([FromQuery] int schoolId)
        {
            try
            {
                var subjects = await _context.Subjects
                    .Include(s => s.Class)
                    .Where(c => c.IsDeleted == 0 && c.SchoolId == schoolId)
                    .Select(s => new
                    {
                        s.SubjectId,
                        s.SubjectName,
                        s.ClassId,
                        s.SchoolId,
                        s.CreatedAt,
                        className = s.Class.ClassName
                    })
                    .ToListAsync();

                return Ok(new { data = subjects });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving subjects",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }

        [HttpPost("CreateSubject")]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectDto dto)
        {
            try
            {
                // Check for existing section
                var existingSubject = await _context.Subjects
                    .FirstOrDefaultAsync(s =>
                        s.SubjectName.ToLower() == dto.SubjectName.ToLower() &&
                        s.ClassId == dto.ClassId &&
                        s.SchoolId == dto.SchoolId &&
                        s.IsDeleted == 0);

                if (existingSubject != null)
                {
                    return Conflict(new
                    {
                        message = "Subject with the same name already exists for this class and school"
                    });
                }
                var subject = new Subject
                {
                    SubjectName = dto.SubjectName,
                    ClassId = dto.ClassId,
                    SchoolId = dto.SchoolId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = 0
                };

                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Subject created successfully",
                    subject
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error creating subject",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }

        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject([FromBody] SubjectDto dto)
        {
            try
            {
                if (dto.SubjectId == null)
                    return BadRequest("Subject ID is required.");

                var subject = await _context.Subjects.FindAsync(dto.SubjectId.Value);
                if (subject == null)
                    return NotFound("Subject not found");

                subject.SubjectName = dto.SubjectName;
                subject.ClassId = dto.ClassId;
                subject.SchoolId = dto.SchoolId;
                subject.IsDeleted = 0;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Subject updated successfully",
                    subject
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating subject",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }

        [HttpGet("GetSubjectsByClassId")]
        public async Task<IActionResult> GetSubjectsByClassId([FromQuery] int classId)
        {
            try
            {
                var subjects = await _context.Subjects
                    .Include(s => s.Class)
                    .Where(s => s.IsDeleted == 0 && s.ClassId == classId)
                    .Select(s => new
                    {
                        s.SubjectId,
                        s.SubjectName,
                        s.ClassId,
                        s.SchoolId,
                        s.CreatedAt,
                        className = s.Class.ClassName
                    })
                    .ToListAsync();

                if (subjects == null || subjects.Count == 0)
                {
                    return NotFound(new
                    {
                        message = "No subjects found for the given class ID."
                    });
                }

                return Ok(new { data = subjects });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving subjects by class ID",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }


        [HttpDelete("DeleteSubject/{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                var subject = await _context.Subjects.FindAsync(id);
                if (subject == null)
                    return NotFound("Subject not found");

                subject.IsDeleted = 1;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Subject deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error deleting subject",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }
    }
}
