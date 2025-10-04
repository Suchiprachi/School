using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;
using Secu_School_API.DTOs;
using Microsoft.Extensions.Logging;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopyController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        public BookCopyController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookCopies()
        {
            try
            {
                var copies = await _context.BookCopies
                    .Include(bc => bc.Book)
                    .Include(bc => bc.Location)
                    .ToListAsync();

                var result = copies.Select(c => ToDto(c)).ToList();
                return Ok(new { data = result }); // <- Wrap in { data: ... }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving book copies.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookCopy(int id)
        {
            try
            {
                var copy = await _context.BookCopies
                    .Include(bc => bc.Book)
                    .Include(bc => bc.Location)
                    .FirstOrDefaultAsync(bc => bc.CopyId == id);

                if (copy == null)
                    return NotFound(new { message = "Book copy not found." });

                return Ok(ToDto(copy));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving book copy.", error = ex.Message });
            }
        }

        [HttpPost("create-bookcopy")]
        public async Task<IActionResult> CreateBookCopy([FromBody] BookCopyDto dto)
        {
            try
            {
                var entity = ToEntity(dto);
                _context.BookCopies.Add(entity);
                await _context.SaveChangesAsync();

                dto.CopyId = entity.CopyId;
                return Ok(new { message = "Book copy created successfully.", bookCopy = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating book copy.", error = ex.Message });
            }
        }

        [HttpPut("update-bookcopy")]
        public async Task<IActionResult> UpdateBookCopy([FromBody] BookCopyDto dto)
        {
            if (dto.CopyId == 0)
                return BadRequest("Copy ID is required.");

            try
            {
                var entity = await _context.BookCopies.FindAsync(dto.CopyId);
                if (entity == null)
                    return NotFound(new { message = "Book copy not found." });

                // Update fields manually
                entity.BookId = dto.BookId;
                entity.Barcode = dto.Barcode;
                entity.LocationId = dto.LocationId;
                entity.Status = dto.Status;
                entity.IsDeleted = dto.IsDeleted;
                entity.IssuedTo = dto.IssuedTo;
                entity.RoleId = dto.RoleId;
                entity.IssuedOn = dto.IssuedOn;
                entity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Book copy updated successfully.", bookCopy = dto });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating book copy.", error = ex.Message });
            }
        }

        [HttpDelete("delete-bookcopy/{id}")]
        public async Task<IActionResult> DeleteBookCopy([FromRoute] int id)
        {
            try
            {
                var entity = await _context.BookCopies.FindAsync(id);
                if (entity == null)
                    return NotFound(new { message = "Book copy not found." });

                _context.BookCopies.Remove(entity);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Book copy deleted successfully.", copyId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting book copy.", error = ex.Message });
            }
        }

        // Add this new endpoint for barcode uniqueness check
        [HttpGet("check-barcode")]
        public async Task<IActionResult> CheckBarcodeUniqueness(
            [FromQuery] string barcode,
            [FromQuery] int excludeCopyId = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                {
                    return BadRequest(new { message = "Barcode is required." });
                }

                bool exists = await _context.BookCopies
                    .AnyAsync(bc =>
                        bc.Barcode == barcode &&
                        bc.CopyId != excludeCopyId);

                return Ok(new { isUnique = !exists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error checking barcode uniqueness.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("bulk-create/{bookId}")]
        public async Task<IActionResult> BulkCreateCopies(int bookId, [FromBody] BulkCopyCreateDto dto)
        {
            try
            {
                var book = await _context.Books.FindAsync(bookId);
                if (book == null) return NotFound("Book not found");

                var newCopies = dto.Barcodes.Select(barcode => new BookCopy
                {
                    BookId = bookId,
                    Barcode = barcode,
                    Status = "available",
                    IsDeleted = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                _context.BookCopies.AddRange(newCopies);
                await _context.SaveChangesAsync();

                // Update book copy counts
                book.TotalCopies += newCopies.Count;
                book.AvailableCopies += newCopies.Count;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"{newCopies.Count} copies created successfully",
                    bookId,
                    newCopyIds = newCopies.Select(c => c.CopyId)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error creating copies",
                    error = ex.Message
                });
            }
        }

        public class BulkCopyCreateDto
        {
            public List<string> Barcodes { get; set; } = new();
        }


        // ===== Helper methods for mapping =====

        private BookCopyDto ToDto(BookCopy copy)
        {
            return new BookCopyDto
            {
                CopyId = copy.CopyId,
                BookId = copy.BookId,
                Barcode = copy.Barcode,
                LocationId = copy.LocationId,
                Status = copy.Status,
                IsDeleted = copy.IsDeleted,
                IssuedTo = copy.IssuedTo,
                RoleId = copy.RoleId,
                IssuedOn = copy.IssuedOn,
                CreatedAt = copy.CreatedAt,
                UpdatedAt = copy.UpdatedAt
            };
        }

        private BookCopy ToEntity(BookCopyDto dto)
        {
            return new BookCopy
            {
                CopyId = dto.CopyId,
                BookId = dto.BookId,
                Barcode = dto.Barcode,
                LocationId = dto.LocationId,
                Status = dto.Status,
                IsDeleted = dto.IsDeleted,
                IssuedTo = dto.IssuedTo,
                RoleId = dto.RoleId,
                IssuedOn = dto.IssuedOn,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }
    }
}
