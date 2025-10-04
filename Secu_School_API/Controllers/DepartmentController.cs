using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public DepartmentController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments
                                                   .Include(d => d.School)
                                                   .Select(d => new DepartmentDto
                                                   {
                                                       DepartmentId = d.DepartmentId,
                                                       DepartmentName = d.DepartmentName,
                                                       Description = d.Description,
                                                       SchoolId = d.SchoolId,
                                                       SchoolName = d.School!.SchoolName
                                                   })
                                                   .ToListAsync();

                return Ok(new { data = departments });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching departments.");
            }
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // POST: api/Department
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            try
            {


                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while saving the department.", details = ex.Message });
            }
        }

        // PUT: api/Department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.DepartmentId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
