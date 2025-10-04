// Updated SectionController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Dtos;
using Secu_School_API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SectionController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetSections/{schoolId}")]
        public async Task<IActionResult> GetSections(int schoolId)
        {
            try
            {
                var sections = await _context.Sections
                    .Include(s => s.Class)
                    .Where(s => s.SchoolId == schoolId && s.IsDeleted == 0)
                    .Select(s => new
                    {
                        s.SectionId,
                        s.SectionName,
                        s.ClassId,
                        s.SchoolId,
                        className = s.Class.ClassName
                    })
                    .ToListAsync();

                return Ok(new { data = sections });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving sections",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }
        [HttpGet("GetSectionsByClassId/{classId}/{schoolId}")]
        public async Task<IActionResult> GetSectionsByClassId(int classId, int schoolId)
        {
            var sections = await _context.Sections
                .Where(s => s.ClassId == classId)
                .ToListAsync();

            return Ok(sections);
        }
        [HttpPost("CreateSection")]
        public async Task<IActionResult> CreateSection([FromBody] SectionDto dto)
        {
            try
            {
                // Check for existing section
                var existingSection = await _context.Sections
                    .FirstOrDefaultAsync(s =>
                        s.SectionName.ToLower() == dto.SectionName.ToLower() &&
                        s.ClassId == dto.ClassId &&
                        s.SchoolId == dto.SchoolId &&
                        s.IsDeleted == 0);

                if (existingSection != null)
                {
                    return Conflict(new
                    {
                        message = "Section with the same name already exists for this class and school"
                    });
                }

                var section = new Section
                {
                    SectionName = dto.SectionName,
                    ClassId = dto.ClassId,
                    SchoolId = dto.SchoolId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = 0
                };

                _context.Sections.Add(section);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Section created successfully",
                    section
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error creating section",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }

        [HttpPut("UpdateSection")]
        public async Task<IActionResult> UpdateSection([FromBody] SectionDto dto)
        {
            try
            {
                if (dto.SectionId == null)
                    return BadRequest("Section ID is required.");

                var section = await _context.Sections.FindAsync(dto.SectionId.Value);
                if (section == null || section.IsDeleted == 1)
                    return NotFound("Section not found");

                section.SectionName = dto.SectionName;
                section.ClassId = dto.ClassId;
                section.SchoolId = dto.SchoolId;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Section updated successfully",
                    section
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating section",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }

        [HttpDelete("DeleteSection/{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            try
            {
                var section = await _context.Sections.FindAsync(id);
                if (section == null || section.IsDeleted == 1)
                    return NotFound("Section not found");

                section.IsDeleted = 1;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Section deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error deleting section",
                    error = ex.Message,
                    full = ex.ToString()
                });
            }
        }
    }
}