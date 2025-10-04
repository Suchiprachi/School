using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public LeaveTypeController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
        {
            return await _context.LeaveTypes.ToListAsync();
        }

        // GET: api/LeaveType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return leaveType;
        }

        // POST: api/LeaveType
        [HttpPost]
        public async Task<ActionResult<LeaveType>> PostLeaveType(LeaveTypeDto dto)
        {
            var leaveType = new LeaveType
            {
                SchoolId = dto.SchoolId,
                LeaveTypeName = dto.LeaveTypeName,
                Description = dto.Description,
                IsPaid = dto.IsPaid,
                MaxDaysPerYear = dto.MaxDaysPerYear
            };

            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLeaveType), new { id = leaveType.LeaveTypeId }, leaveType);
        }

        // PUT: api/LeaveType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(int id, LeaveTypeDto dto)
        {
            var existing = await _context.LeaveTypes.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.SchoolId = dto.SchoolId;
            existing.LeaveTypeName = dto.LeaveTypeName;
            existing.Description = dto.Description;
            existing.IsPaid = dto.IsPaid;
            existing.MaxDaysPerYear = dto.MaxDaysPerYear;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/LeaveType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
