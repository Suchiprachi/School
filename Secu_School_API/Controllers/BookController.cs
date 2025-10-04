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
    public class BookController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public BookController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .Include(b => b.School)
                    .Include(b => b.Language)  // Added Language include
                    .Where(b => b.IsDeleted==0)   // Added soft delete filter
                    .Select(b => new
                    {
                        b.BookId,
                        b.Title,
                        b.ISBN,
                        AuthorId = b.AuthorId,
                        AuthorName = b.Author != null ? b.Author.Name : null,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category != null ? b.Category.CategoryName : null,
                        PublisherId = b.PublisherId,
                        PublisherName = b.Publisher != null ? b.Publisher.PublisherName : null,
                        b.YearPublished,
                        b.TotalCopies,
                        b.AvailableCopies,
                        b.SchoolId,
                        SchoolName = b.School != null ? b.School.SchoolName : null,
                        b.CreatedAt,
                        b.UpdatedAt,
                        b.Edition,
                        LanguageId = b.LanguageId,  // Added LanguageId
                        LanguageName = b.Language != null ? b.Language.LanguageName : null,  // Added LanguageName
                        b.Description,
                        b.NumberOfPages,
                        b.Status,
                        b.IsDeleted
                    })
                    .ToListAsync();

                return Ok(new { message = "Books retrieved successfully", data = books });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving books: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving books",
                    error = ex.Message
                });
            }
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .Include(b => b.School)
                    .Include(b => b.Language)  // Added Language include
                    .Where(b => b.BookId == id && b.IsDeleted==0)  // Added soft delete filter
                    .Select(b => new
                    {
                        b.BookId,
                        b.Title,
                        b.ISBN,
                        AuthorId = b.AuthorId,
                        AuthorName = b.Author != null ? b.Author.Name : null,
                        CategoryId = b.CategoryId,
                        CategoryName = b.Category != null ? b.Category.CategoryName : null,
                        PublisherId = b.PublisherId,
                        PublisherName = b.Publisher != null ? b.Publisher.PublisherName : null,
                        b.YearPublished,
                        b.TotalCopies,
                        b.AvailableCopies,
                        b.SchoolId,
                        SchoolName = b.School != null ? b.School.SchoolName : null,
                        b.CreatedAt,
                        b.UpdatedAt,
                        b.Edition,
                        LanguageId = b.LanguageId,  // Added LanguageId
                        LanguageName = b.Language != null ? b.Language.LanguageName : null,  // Added LanguageName
                        b.Description,
                        b.NumberOfPages,
                        b.Status,
                        b.IsDeleted
                    })
                    .FirstOrDefaultAsync();

                if (book == null)
                    return NotFound(new { message = "Book not found" });

                return Ok(new { message = "Book retrieved successfully", data = book });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving book: {ex}");
                return StatusCode(500, new
                {
                    message = "Error retrieving book",
                    error = ex.Message
                });
            }
        }

        // POST: api/Book
        [HttpPost("create-book")]
        public async Task<IActionResult> CreateBook([FromBody] BookDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate copies
                if (dto.TotalCopies <= 0)
                    return BadRequest(new { message = "Total copies must be at least 1" });

                if (dto.YearPublished.HasValue &&
                   (dto.YearPublished < 1800 || dto.YearPublished > DateTime.Now.Year))
                {
                    return BadRequest(new { message = $"Year published must be between 1800 and {DateTime.Now.Year}" });
                }

                var book = new Book
                {
                    Title = dto.Title,
                    ISBN = dto.ISBN,
                    AuthorId = dto.AuthorId,
                    CategoryId = dto.CategoryId,
                    PublisherId = dto.PublisherId,
                    YearPublished = dto.YearPublished,
                    TotalCopies = dto.TotalCopies,
                    AvailableCopies = dto.TotalCopies, // All copies are available at creation
                    SchoolId = dto.SchoolId,
                    Edition = dto.Edition,
                    LanguageId = dto.LanguageId,
                    Description = dto.Description,
                    NumberOfPages = dto.NumberOfPages,
                    Status = dto.Status,
                    IsDeleted = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                // Generate BookCopies with barcodes
                for (int i = 1; i <= dto.TotalCopies; i++)
                {
                    string barcode = GenerateBarcode(book, i);

                    _context.BookCopies.Add(new BookCopy
                    {
                        BookId = book.BookId,
                        Barcode = barcode,
                        Status = "available",
                        IsDeleted = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Book and copies created successfully",
                    bookId = book.BookId
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error creating book and copies: {ex}");
                return StatusCode(500, new
                {
                    message = "Error creating book and copies",
                    error = ex.Message
                });
            }
        }

        private string GenerateBarcode(Book book, int copyNumber)
        {
            // Example barcode format: "BOOK-<BookId>-COPY-<CopyNumber>"
            return $"BOOK-{book.BookId:D5}-COPY-{copyNumber:D3}";
        }



        // PUT: api/Book/5
        [HttpPut("update-book/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto dto)
        {
            if (id != dto.BookId)
                return BadRequest(new { message = "Book ID mismatch" });

            try
            {
                // Validate copies
                if (dto.TotalCopies <= 0)
                    return BadRequest(new { message = "Total copies must be at least 1" });

                if (dto.AvailableCopies < 0 || dto.AvailableCopies > dto.TotalCopies)
                    return BadRequest(new { message = "Available copies must be between 0 and total copies" });

                // Validate year
                if (dto.YearPublished.HasValue &&
                   (dto.YearPublished < 1800 || dto.YearPublished > DateTime.Now.Year))
                {
                    return BadRequest(new { message = $"Year published must be between 1800 and {DateTime.Now.Year}" });
                }

                var book = await _context.Books.FindAsync(id);
                if (book == null || book.IsDeleted==1)
                    return NotFound(new { message = "Book not found" });

                book.Title = dto.Title;
                book.ISBN = dto.ISBN;
                book.AuthorId = dto.AuthorId;
                book.CategoryId = dto.CategoryId;
                book.PublisherId = dto.PublisherId;
                book.YearPublished = dto.YearPublished;
                book.TotalCopies = dto.TotalCopies;
                book.AvailableCopies = dto.AvailableCopies;
                book.SchoolId = dto.SchoolId;
                book.Edition = dto.Edition;
                book.LanguageId = dto.LanguageId;  // Added LanguageId
                book.Description = dto.Description;
                book.NumberOfPages = dto.NumberOfPages;
                book.Status = dto.Status;
                book.UpdatedAt = DateTime.UtcNow;

                _context.Entry(book).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Book updated successfully",
                    book = new
                    {
                        book.BookId,
                        dto.Title,
                        dto.ISBN,
                        dto.AuthorId,
                        dto.CategoryId,
                        dto.PublisherId,
                        dto.YearPublished,
                        dto.TotalCopies,
                        dto.AvailableCopies,
                        dto.SchoolId,
                        dto.Edition,
                        dto.LanguageId,
                        dto.Description,
                        dto.NumberOfPages,
                        dto.Status,
                        book.IsDeleted
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating book: {ex}");
                return StatusCode(500, new
                {
                    message = "Error updating book",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Book/5 (Soft Delete)
        [HttpDelete("delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null || book.IsDeleted == 1)
                {
                    return NotFound(new { message = "Book not found" });
                }

                // Soft delete
                book.IsDeleted = 1;
                book.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Book soft deleted successfully",
                    bookId = id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting book: {ex}");
                return StatusCode(500, new
                {
                    message = "Error deleting book",
                    error = ex.Message
                });
            }
        }

        // Restore soft-deleted book
        [HttpPatch("restore-book/{id}")]
        public async Task<IActionResult> RestoreBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                    return NotFound(new { message = "Book not found" });

                if (book.IsDeleted==0)
                    return BadRequest(new { message = "Book is not deleted" });

                book.IsDeleted = 0;
                book.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Book restored successfully",
                    bookId = id
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restoring book: {ex}");
                return StatusCode(500, new
                {
                    message = "Error restoring book",
                    error = ex.Message
                });
            }
        }
    }
}