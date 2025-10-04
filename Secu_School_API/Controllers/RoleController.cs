using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public RolesController(SchoolDbContext context)
        {
            _context = context;
        }
        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            try
            {
                return await _context.Roles
                    .Select(r => new RoleDTO
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName,
                        SchoolId = r.SchoolId
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDTO dto)
        {
            try
            {
                var exists = await _context.Roles.AnyAsync(r =>
                    r.RoleName.ToLower() == dto.RoleName.ToLower() &&
                    r.SchoolId == dto.SchoolId);

                if (exists)
                    return BadRequest($"Role '{dto.RoleName}' already exists.");

                var role = new Role
                {
                    RoleName = dto.RoleName,
                    SchoolId = dto.SchoolId
                };

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return Ok(role.RoleId); // return new ID if useful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return NotFound();

                return new RoleDTO
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    SchoolId = role.SchoolId
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, RoleDTO dto)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return NotFound();

                role.RoleName = dto.RoleName;
                role.SchoolId = dto.SchoolId;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return NotFound();

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
