using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;
using Secu_School_API.DTOs;
using System.Security.Claims;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ClassController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateClass")]
        public async Task<IActionResult> CreateClass([FromBody] ClassDto dto)
        {
            try
            {
                var newClass = new Class
                {
                    ClassName = dto.ClassName,
                    SchoolId = dto.SchoolId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Class created successfully", data = newClass });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating class.", error = ex.Message });
            }
        }

        [HttpGet("GetClasses")]
        public async Task<IActionResult> GetClasses([FromQuery] int schoolId)
        {
            try
            {
                var classes = await _context.Classes
                    .Where(c => c.IsDeleted == 0 && c.SchoolId == schoolId)
                    .ToListAsync();

                return Ok(new { success = true, data = classes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Failed to fetch classes.", error = ex.Message });
            }
        }

        [HttpGet("GetClassById/{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            try
            {
                var result = await _context.Classes.FindAsync(id);
                if (result == null || result.IsDeleted==0)
                    return NotFound(new { success = false, message = "Class not found" });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Failed to fetch class by ID.", error = ex.Message });
            }
        }

        [HttpPut("UpdateClass")]
        public async Task<IActionResult> UpdateClass([FromBody] Class updatedClass)
        {
            try
            {
                var existing = await _context.Classes.FindAsync(updatedClass.ClassId);
                if (existing == null || existing.IsDeleted == 1)
                    return NotFound(new { success = false, message = "Class not found" });

                existing.ClassName = updatedClass.ClassName;
                existing.SchoolId = updatedClass.SchoolId;

                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Class updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating class.", error = ex.Message });
            }
        }

        [HttpDelete("DeleteClass/{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                var existing = await _context.Classes.FindAsync(id);
                if (existing == null || existing.IsDeleted == 1)
                    return NotFound(new { success = false, message = "Class not found" });

                existing.IsDeleted = 1;

                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Class deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting class.", error = ex.Message });
            }
        }
    }
}
