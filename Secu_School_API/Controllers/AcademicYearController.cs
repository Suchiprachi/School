using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secu_School_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicYearController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public AcademicYearController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/AcademicYear
        [HttpGet("GetAcademicYears/{SchoolId}")]
        public async Task<ActionResult<IEnumerable<AcademicYearDto>>> GetAcademicYears(int SchoolId)
        {
            try
            {
                var years = await _context.AcademicYears
                    .Where(p=>p.SchoolId==SchoolId && p.IsDeleted==0)
                    .ToListAsync();

                return Ok(years);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/AcademicYear/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AcademicYearDto>> GetAcademicYear(int id)
        {
            try
            {
                var year = await _context.AcademicYears.FindAsync(id);

                if (year == null)
                    return NotFound();

                return new AcademicYearDto
                {
                    AcademicYearId = year.AcademicYearId,
                    YearName = year.YearName,
                    StartDate = year.StartDate,
                    EndDate = year.EndDate,
                    IsCurrent = year.IsCurrent,
                    SchoolId = year.SchoolId
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/AcademicYear
        [HttpPost("CreateAcademicYear")]
        public async Task<ActionResult<AcademicYearDto>> CreateAcademicYear([FromBody] AcademicYearDto dto)
        {
            try
            {
                var entity = new AcademicYear
                {
                    YearName = dto.YearName,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    IsCurrent = dto.IsCurrent,
                    SchoolId = dto.SchoolId
                };

                _context.AcademicYears.Add(entity);
                await _context.SaveChangesAsync();

                dto.AcademicYearId = entity.AcademicYearId;

                return CreatedAtAction(nameof(GetAcademicYear), new { id = dto.AcademicYearId }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/AcademicYear/5
        [HttpPut("UpdateAcademicYear/{id}")]
        public async Task<IActionResult> UpdateAcademicYear(int id, AcademicYearDto dto)
        {
            try
            {
                if (id != dto.AcademicYearId)
                    return BadRequest();

                var entity = await _context.AcademicYears.FindAsync(id);
                if (entity == null)
                    return NotFound();

                entity.YearName = dto.YearName;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.IsCurrent = dto.IsCurrent;
                entity.SchoolId = dto.SchoolId;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/AcademicYear/5
        [HttpDelete("DeleteAcademicYear/{id}")]
        public async Task<IActionResult> DeleteAcademicYear(int id)
        {
            try
            {
                var entity = await _context.AcademicYears.FindAsync(id);
                if (entity == null)
                    return NotFound();
                entity.IsDeleted = 1;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Academic Year deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
