using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTypeController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public EmployeeTypeController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetEmployeeTypes")]
        public async Task<ActionResult<IEnumerable<EmployeeType>>> GetAll()
        {
            try
            {
                return await _context.EmployeeTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeType>> GetById(int id)
        {
            try
            {
                var type = await _context.EmployeeTypes.FindAsync(id);
                if (type == null) return NotFound();
                return type;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("CreateEmployeeType")]
        public async Task<ActionResult<EmployeeType>> Create(EmployeeType empType)
        {
            try
            {
                _context.EmployeeTypes.Add(empType);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = empType.EmployeeTypeId }, empType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateEmployeeType/{id}")] 
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeType empType)
        {
            if (id != empType.EmployeeTypeId)
                return BadRequest();

            try
            {
                _context.Entry(empType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _context.EmployeeTypes.FindAsync(id);
                if (item == null) return NotFound();

                _context.EmployeeTypes.Remove(item);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

