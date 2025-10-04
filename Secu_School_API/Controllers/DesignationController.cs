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
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public DesignationController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesignations()
         {
            try
            {
                var designations = await _context.Designations
                    .Include(d => d.Department)
                    .Include(d => d.School)
                    .Select(d => new
                    {
                        d.DesignationId,
                        d.DesignationName,
                        d.DepartmentId,
                        d.Description,
                        d.SchoolId
                    })
                    .ToListAsync();

                return Ok(designations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving designations: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving designations.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id)
        {
            try
            {
                var designation = await _context.Designations
                    .Include(d => d.Department)
                    .Include(d => d.School)
                    .Where(d => d.DesignationId == id)
                    .Select(d => new
                    {
                        d.DesignationId,
                        d.DesignationName,
                        d.DepartmentId,
                        d.Description,
                        d.SchoolId
                    })
                    .FirstOrDefaultAsync();

                if (designation == null)
                    return NotFound(new { message = "Designation not found." });

                return Ok(designation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving designation: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving designation.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetDesignationsByDepartment(int departmentId)
        {
            try
            {
                var designations = await _context.Designations
                    .Where(d => d.DepartmentId == departmentId)
                    .Select(d => new
                    {
                        d.DesignationId,
                        d.DesignationName,
                        d.DepartmentId,
                        d.Description,
                        d.SchoolId
                    })
                    .ToListAsync();

                if (designations == null || !designations.Any())
                    return NotFound(new { message = "No designations found for the department." });

                return Ok(designations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving designations: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving designations by department.",
                    error = ex.Message
                });
            }
        }


        [HttpPost("create-designation")]
        public async Task<IActionResult> CreateDesignation([FromBody] DesignationDto dto)
        {
            try
            {
                Console.WriteLine($"Received create request: {System.Text.Json.JsonSerializer.Serialize(dto)}");

                var entity = new Designation
                {
                    DesignationName = dto.DesignationName,
                    DepartmentId = dto.DepartmentId,
                    Description = dto.Description,
                    SchoolId = dto.SchoolId
                };

                _context.Designations.Add(entity);
                await _context.SaveChangesAsync();

                dto.DesignationId = entity.DesignationId;

                return Ok(new { message = "Designation created successfully.", designation = dto });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating designation: {ex}");
                return BadRequest(new
                {
                    message = "Error creating designation.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("update-designation")]
        public async Task<IActionResult> UpdateDesignation([FromForm] DesignationDto dto)
        {
            if (dto.DesignationId == null)
                return BadRequest("Designation ID is required.");

            try
            {
                var entity = await _context.Designations.FindAsync(dto.DesignationId.Value);
                if (entity == null)
                    return NotFound(new { message = "Designation not found." });

                entity.DesignationName = dto.DesignationName;
                entity.DepartmentId = dto.DepartmentId;
                entity.Description = dto.Description;
                entity.SchoolId = dto.SchoolId;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Designation updated successfully.", designation = dto });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating designation: {ex}");
                return BadRequest(new
                {
                    message = "Error updating designation.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("delete-designation/{id}")]
        public async Task<IActionResult> DeleteDesignation([FromRoute] int id)
        {
            try
            {
                var entity = await _context.Designations.FindAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Designation not found." });

                _context.Designations.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Designation deleted successfully.", designationId = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting designation: {ex}");
                return BadRequest(new
                {
                    message = "Error deleting designation.",
                    error = ex.Message
                });
            }
        }

        private bool DesignationExists(int id)
        {
            return _context.Designations.Any(d => d.DesignationId == id);
        }
    }
}
