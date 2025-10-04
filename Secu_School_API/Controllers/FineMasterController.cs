using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineMasterController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public FineMasterController(SchoolDbContext context)
        {
            _context = context;
        }

        // ✅ GET all fine records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FineMasterDTO>>> GetFines()
        {
            return await _context.FineMasters
                .Include(f => f.Role)
                .Select(f => new FineMasterDTO
                {
                    FineId = f.FineId,
                    SchoolId = f.SchoolId,
                    RoleId = f.RoleId,
                    FinePerDay = f.FinePerDay,
                    GracePeriod = f.GracePeriod,
                    MaxFine = f.MaxFine,
                    Status = f.Status,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt,
                    RoleName = f.Role.RoleName
                }).ToListAsync();
        }

        // ✅ GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<FineMasterDTO>> GetFine(int id)
        {
            var fine = await _context.FineMasters
                .Include(f => f.Role)
                .FirstOrDefaultAsync(f => f.FineId == id);

            if (fine == null)
                return NotFound();

            return new FineMasterDTO
            {
                FineId = fine.FineId,
                SchoolId = fine.SchoolId,
                RoleId = fine.RoleId,
                FinePerDay = fine.FinePerDay,
                GracePeriod = fine.GracePeriod,
                MaxFine = fine.MaxFine,
                Status = fine.Status,
                CreatedAt = fine.CreatedAt,
                UpdatedAt = fine.UpdatedAt,
                RoleName = fine.Role?.RoleName
            };
        }

        // ✅ POST (Create)
        [HttpPost]
        public async Task<ActionResult> CreateFine(FineMasterDTO dto)
        {
            var fine = new FineMaster
            {
                SchoolId = dto.SchoolId,
                RoleId = dto.RoleId,
                FinePerDay = dto.FinePerDay,
                GracePeriod = dto.GracePeriod,
                MaxFine = dto.MaxFine,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.FineMasters.Add(fine);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ✅ PUT (Update)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFine(int id, FineMasterDTO dto)
        {
            var fine = await _context.FineMasters.FindAsync(id);
            if (fine == null)
                return NotFound();

            fine.RoleId = dto.RoleId;
            fine.FinePerDay = dto.FinePerDay;
            fine.GracePeriod = dto.GracePeriod;
            fine.MaxFine = dto.MaxFine;
            fine.Status = dto.Status;
            fine.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFine(int id)
        {
            var fine = await _context.FineMasters.FindAsync(id);
            if (fine == null)
                return NotFound();

            _context.FineMasters.Remove(fine);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
