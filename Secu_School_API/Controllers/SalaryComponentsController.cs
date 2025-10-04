using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;


namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryComponentsController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SalaryComponentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/SalaryComponents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryComponent>>> GetSalaryComponents()
        {
            try
            {
                return await _context.SalaryComponents.ToListAsync();
            }
            catch(Exception ex)
            {
                return NoContent();
            }
            
        }

        // GET: api/SalaryComponents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalaryComponent>> GetSalaryComponent(int id)
        {
            var salaryComponent = await _context.SalaryComponents.FindAsync(id);

            if (salaryComponent == null)
            {
                return NotFound();
            }

            return salaryComponent;
        }

        // POST: api/SalaryComponents
        [HttpPost]
        public async Task<ActionResult<SalaryComponent>> PostSalaryComponent(SalaryComponent salaryComponent)
        {
            _context.SalaryComponents.Add(salaryComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalaryComponent", new { id = salaryComponent.ComponentId }, salaryComponent);
        }

        // PUT: api/SalaryComponents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalaryComponent(int id, SalaryComponent salaryComponent)
        {
            if (id != salaryComponent.ComponentId)
            {
                return BadRequest();
            }

            _context.Entry(salaryComponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryComponentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/SalaryComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalaryComponent(int id)
        {
            var salaryComponent = await _context.SalaryComponents.FindAsync(id);
            if (salaryComponent == null)
            {
                return NotFound();
            }

            _context.SalaryComponents.Remove(salaryComponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaryComponentExists(int id)
        {
            return _context.SalaryComponents.Any(e => e.ComponentId == id);
        }
    }
}
