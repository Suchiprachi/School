using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public LocationController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            try
            {
                var locations = await _context.Locations
                    .Include(l => l.School)
                    .Select(l => new LocationDto
                    {
                        LocationId = l.LocationId,
                        LocationName = l.LocationName,
                        SchoolId = l.SchoolId,
                        SchoolName = l.School!.SchoolName,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new { data = locations });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching locations.");
            }
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            try
            {
                var location = await _context.Locations
                    .Include(l => l.School)
                    .Where(l => l.LocationId == id)
                    .Select(l => new LocationDto
                    {
                        LocationId = l.LocationId,
                        LocationName = l.LocationName,
                        SchoolId = l.SchoolId,
                        SchoolName = l.School!.SchoolName,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the location.");
            }
        }

        // POST: api/Location
        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(LocationDto locationDto)
        {
            try
            {
                var location = new Location
                {
                    LocationName = locationDto.LocationName,
                    SchoolId = locationDto.SchoolId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLocation), new { id = location.LocationId }, location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while creating the location.",
                    details = ex.Message
                });
            }
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationDto locationDto)
        {
            if (id != locationDto.LocationId)
            {
                return BadRequest("Location ID mismatch");
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            try
            {
                location.LocationName = locationDto.LocationName;
                location.SchoolId = locationDto.SchoolId;
                location.UpdatedAt = DateTime.UtcNow;

                _context.Entry(location).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                return StatusCode(500, new
                {
                    error = "Concurrency error occurred while updating the location.",
                    details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while updating the location.",
                    details = ex.Message
                });
            }
        }

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while deleting the location.",
                    details = ex.Message
                });
            }
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}