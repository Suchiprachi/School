using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Models;
using Secu_School_API.Models.DTOs;

namespace Secu_School_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly ILogger<ExamController> _logger;

        public ExamController(SchoolDbContext context, ILogger<ExamController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ Get all exams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExams()
        {
            try
            {
                var exams = await _context.Exams
                    .Include(e => e.School)
                    .Include(e => e.AcademicYear)
                    .Select(e => new ExamDto
                    {
                        ExamId = e.ExamId,
                        SchoolId = e.SchoolId,
                        SchoolName = e.School.SchoolName, // Added for front-end
                        AcademicYearId = e.AcademicYearId,
                        AcademicYearName = e.AcademicYear.YearName, // Added for front-end
                        ExamName = e.ExamName,
                        Description = e.Description,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    })
                    .ToListAsync();

                return Ok(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching exams.");
                return StatusCode(500, "An error occurred while retrieving exams.");
            }
        }

        // ✅ Get exam by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> GetExam(int id)
        {
            try
            {
                var exam = await _context.Exams
                    .Include(e => e.School)
                    .Include(e => e.AcademicYear)
                    .FirstOrDefaultAsync(e => e.ExamId == id);

                if (exam == null)
                    return NotFound("Exam not found.");

                var dto = new ExamDto
                {
                    ExamId = exam.ExamId,
                    SchoolId = exam.SchoolId,
                    SchoolName = exam.School.SchoolName,
                    AcademicYearId = exam.AcademicYearId,
                    AcademicYearName = exam.AcademicYear.YearName,
                    ExamName = exam.ExamName,
                    Description = exam.Description,
                    StartDate = exam.StartDate,
                    EndDate = exam.EndDate
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching exam with ID {id}");
                return StatusCode(500, "An error occurred while retrieving the exam.");
            }
        }

        // ✅ Create exam
        [HttpPost]
        public async Task<ActionResult<ExamDto>> CreateExam([FromBody] ExamDto examDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Ensure StartDate <= EndDate
                if (examDto.StartDate > examDto.EndDate)
                    return BadRequest("StartDate cannot be after EndDate.");

                var exam = new Exam
                {
                    SchoolId = examDto.SchoolId,
                    AcademicYearId = examDto.AcademicYearId,
                    ExamName = examDto.ExamName,
                    Description = examDto.Description,
                    StartDate = examDto.StartDate,
                    EndDate = examDto.EndDate,
                    CreatedOn = DateTime.UtcNow
                };

                _context.Exams.Add(exam);
                await _context.SaveChangesAsync();

                examDto.ExamId = exam.ExamId;

                return CreatedAtAction(nameof(GetExam), new { id = exam.ExamId }, examDto);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error creating exam.");
                return StatusCode(500, "Database error: possibly invalid foreign key or null value.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating exam.");
                return StatusCode(500, "An unexpected error occurred while creating the exam.");
            }
        }


        // ✅ Update exam
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExam(int id, [FromBody] ExamDto examDto)
        {
            try
            {
                if (id != examDto.ExamId)
                    return BadRequest("Exam ID mismatch.");

                var exam = await _context.Exams.FindAsync(id);
                if (exam == null)
                    return NotFound("Exam not found.");

                // Update fields
                exam.SchoolId = examDto.SchoolId;
                exam.AcademicYearId = examDto.AcademicYearId;
                exam.ExamName = examDto.ExamName;
                exam.Description = examDto.Description;
                exam.StartDate = examDto.StartDate;
                exam.EndDate = examDto.EndDate;

                _context.Exams.Update(exam);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating exam with ID {id}");
                return StatusCode(500, "An error occurred while updating the exam.");
            }
        }
        [HttpGet("BySchool/{schoolId}")]
        public async Task<ActionResult<IEnumerable<ExamDto>>> GetExamsBySchool(int schoolId)
        {
            try
            {
                var exams = await _context.Exams
                    .Where(e => e.SchoolId == schoolId)
                    .Include(e => e.AcademicYear)
                    .Include(e => e.School)
                    .Select(e => new ExamDto
                    {
                        ExamId = e.ExamId,
                        SchoolId = e.SchoolId,
                        SchoolName = e.School.SchoolName,
                        AcademicYearId = e.AcademicYearId,
                        AcademicYearName = e.AcademicYear.YearName,
                        ExamName = e.ExamName,
                        Description = e.Description,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    })
                    .ToListAsync();

                return Ok(exams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching exams for SchoolId {schoolId}");
                return StatusCode(500, "An error occurred while retrieving exams.");
            }
        }

        // ✅ Delete exam
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            try
            {
                var exam = await _context.Exams
                    .Include(e => e.ExamSubjects) // or any child table
                    .FirstOrDefaultAsync(e => e.ExamId == id);

                if (exam == null)
                    return NotFound("Exam not found.");

                if (exam.ExamSubjects.Any())
                    return BadRequest("Cannot delete exam because it has related subjects.");

                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Database error deleting exam with ID {id}");
                return StatusCode(500, "Cannot delete exam due to database constraints.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error deleting exam with ID {id}");
                return StatusCode(500, "An unexpected error occurred while deleting the exam.");
            }
        }

    }
}
