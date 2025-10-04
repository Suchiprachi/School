using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;
using Secu_School_API.Models.DTOs;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamSubjectController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ExamSubjectController(SchoolDbContext context)
        {
            _context = context;
        }

        // ✅ Create Exam Subject
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ExamSubjectDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid exam subject data" });

            try
            {
                // -------- Duplicate Check --------
                bool exists = await _context.ExamSubjects.AnyAsync(e =>
                    e.ExamId == dto.ExamId &&
                    e.ClassId == dto.ClassId &&
                    e.SectionId == dto.SectionId &&
                    e.SubjectId == dto.SubjectId &&
                    e.StaffId == dto.StaffId
                );

                if (exists)
                    return Conflict(new { message = "This exam-subject mapping already exists!" });

                var examSubject = new ExamSubject
                {
                    ExamId = dto.ExamId,
                    ClassId = dto.ClassId,
                    SectionId = dto.SectionId,
                    SubjectId = dto.SubjectId,
                    StaffId = dto.StaffId,
                    ExamDate = dto.ExamDate,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    MaxMarks = dto.MaxMarks,
                    PassingMarks = dto.PassingMarks
                };

                _context.ExamSubjects.Add(examSubject);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Exam subject created successfully", data = examSubject });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating exam subject", error = ex.Message });
            }
        }

        // ✅ Get All Exam Subjects
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.ExamSubjects.AsNoTracking()
                    .Include(x => x.Exam)
                    .Include(x => x.Class)
                    .Include(x => x.Section)
                    .Include(x => x.Subject)
                    .Include(x => x.Staff)
                    .Select(x => new
                    {
                        x.ExamSubjectId,
                        x.ExamId,
                        ExamName = x.Exam.ExamName,
                        x.ClassId,
                        ClassName = x.Class.ClassName,
                        x.SectionId,
                        SectionName = x.Section.SectionName,
                        x.SubjectId,
                        SubjectName = x.Subject.SubjectName,
                        x.StaffId,
                        StaffName = x.Staff.Name,
                        x.ExamDate,
                        x.StartTime,
                        x.EndTime,
                        x.MaxMarks,
                        x.PassingMarks
                    })
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error fetching exam subjects", error = ex.Message });
            }
        }

        // ✅ Get Exam Subject by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var examSubject = await _context.ExamSubjects
                    .AsNoTracking()
                    .Include(x => x.Exam)
                    .Include(x => x.Class)
                    .Include(x => x.Section)
                    .Include(x => x.Subject)
                    .Include(x => x.Staff)
                    .Where(x => x.ExamSubjectId == id)
                    .Select(x => new
                    {
                        x.ExamSubjectId,
                        x.ExamId,
                        ExamName = x.Exam.ExamName,
                        x.ClassId,
                        ClassName = x.Class.ClassName,
                        x.SectionId,
                        SectionName = x.Section.SectionName,
                        x.SubjectId,
                        SubjectName = x.Subject.SubjectName,
                        x.StaffId,
                        StaffName = x.Staff.Name,
                        x.ExamDate,
                        x.StartTime,
                        x.EndTime,
                        x.MaxMarks,
                        x.PassingMarks
                    })
                    .FirstOrDefaultAsync();

                if (examSubject == null)
                    return NotFound(new { message = "Exam subject not found" });

                return Ok(examSubject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching exam subject", error = ex.Message });
            }
        }


        // ✅ Update Exam Subject
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExamSubjectDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid exam subject data" });

            try
            {
                var examSubject = await _context.ExamSubjects.FindAsync(id);
                if (examSubject == null)
                    return NotFound(new { message = "Exam subject not found" });

                // -------- Duplicate Check --------
                bool exists = await _context.ExamSubjects.AnyAsync(e =>
                    e.ExamId == dto.ExamId &&
                    e.ClassId == dto.ClassId &&
                    e.SectionId == dto.SectionId &&
                    e.SubjectId == dto.SubjectId &&
                    e.StaffId == dto.StaffId &&
                    e.ExamSubjectId != id // ignore current record
                );

                if (exists)
                    return Conflict(new { message = "This exam-subject mapping already exists!" });

                examSubject.ExamId = dto.ExamId;
                examSubject.ClassId = dto.ClassId;
                examSubject.SectionId = dto.SectionId;
                examSubject.SubjectId = dto.SubjectId;
                examSubject.StaffId = dto.StaffId;
                examSubject.ExamDate = dto.ExamDate;
                examSubject.StartTime = dto.StartTime;
                examSubject.EndTime = dto.EndTime;
                examSubject.MaxMarks = dto.MaxMarks;
                examSubject.PassingMarks = dto.PassingMarks;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Exam subject updated successfully", data = examSubject });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating exam subject", error = ex.Message });
            }
        }


        // ✅ Delete Exam Subject
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var examSubject = await _context.ExamSubjects.FindAsync(id);
                if (examSubject == null)
                    return NotFound(new { message = "Exam subject not found" });

                _context.ExamSubjects.Remove(examSubject);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Exam subject deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting exam subject", error = ex.Message });
            }
        }
    }
}
