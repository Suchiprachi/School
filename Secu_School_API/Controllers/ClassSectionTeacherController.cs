using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.Dtos;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSectionTeacherController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ClassSectionTeacherController(SchoolDbContext context)
        {
            _context = context;
        }

        // ✅ GET all records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSectionTeacherDto>>> GetAll()
        {
            var entities = await _context.ClassSectionTeachers
                .Include(cst => cst.Class)
                .Include(cst => cst.Section)
                .Include(cst => cst.Staff)
                .Include(cst => cst.Subject)
                .Include(cst => cst.School)
                .Include(cst => cst.AcademicYear)
                .ToListAsync();

            var dtos = entities.Select(entity => new ClassSectionTeacherDto
            {
                Id = entity.Id,
                ClassId = entity.ClassId,
                SectionId = entity.SectionId,
                StaffId = entity.StaffId,
                SubjectId = entity.SubjectId,
                SchoolId = entity.SchoolId,
                AcademicYearId = entity.AcademicYearId,
                IsDeleted = entity.IsDeleted,
                ClassName = entity.Class?.ClassName,
                SectionName = entity.Section?.SectionName,
                SubjectName = entity.Subject?.SubjectName,
                TeacherName = entity.Staff?.Name,
                SchoolName = entity.School?.SchoolName,
                AcademicYearName = entity.AcademicYear?.YearName
            }).ToList();

            return Ok(dtos);
        }


        // ✅ GET by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassSectionTeacherDto>> GetById(int id)
        {
            try
            {
                var entity = await _context.ClassSectionTeachers
                    .Include(cst => cst.Class)
                    .Include(cst => cst.Section)
                    .Include(cst => cst.Staff)
                    .Include(cst => cst.Subject)
                    .Include(cst => cst.School)
                    .Include(cst => cst.AcademicYear)
                    .FirstOrDefaultAsync(cst => cst.Id == id);

                if (entity == null)
                    return NotFound();

                var dto = new ClassSectionTeacherDto
                {
                    Id = entity.Id,
                    ClassId = entity.ClassId,
                    SectionId = entity.SectionId,
                    StaffId = entity.StaffId,
                    SubjectId = entity.SubjectId,
                    SchoolId = entity.SchoolId,
                    AcademicYearId = entity.AcademicYearId,
                    ClassName = entity.Class != null ? entity.Class.ClassName : string.Empty,
                    SectionName = entity.Section != null ? entity.Section.SectionName : string.Empty,
                    SubjectName = entity.Subject != null ? entity.Subject.SubjectName : string.Empty,
                    TeacherName = entity.Staff != null ? $"{entity.Staff.Name}" : string.Empty,
                    SchoolName = entity.School != null ? entity.School.SchoolName : string.Empty,
                    AcademicYearName = entity.AcademicYear != null ? entity.AcademicYear.YearName : string.Empty
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ✅ CREATE
        [HttpPost]
        public async Task<ActionResult<ClassSectionTeacherDto>> Create([FromBody] ClassSectionTeacherDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Invalid data.");

                // Check duplicate
                var exists = await _context.ClassSectionTeachers
                    .AnyAsync(cst =>
                        cst.ClassId == dto.ClassId &&
                        cst.SectionId == dto.SectionId &&
                        cst.StaffId == dto.StaffId &&
                        cst.SubjectId == dto.SubjectId &&
                        cst.SchoolId == dto.SchoolId &&
                        cst.AcademicYearId == dto.AcademicYearId);

                if (exists)
                    return Conflict("This mapping already exists for the academic year.");

                var entity = new ClassSectionTeacher
                {
                    ClassId = dto.ClassId,
                    SectionId = dto.SectionId,
                    StaffId = dto.StaffId,
                    SubjectId = dto.SubjectId,
                    SchoolId = dto.SchoolId,
                    AcademicYearId = dto.AcademicYearId,
                    IsDeleted = 0
                };

                _context.ClassSectionTeachers.Add(entity);
                await _context.SaveChangesAsync();

                // ✅ Convert to DTO
                var response = new ClassSectionTeacherDto
                {
                    Id = entity.Id,
                    ClassId = entity.ClassId,
                    SectionId = entity.SectionId,
                    StaffId = entity.StaffId,
                    SubjectId = entity.SubjectId,
                    SchoolId = entity.SchoolId,
                    AcademicYearId = entity.AcademicYearId,
                    ClassName = (await _context.Classes.FindAsync(entity.ClassId))?.ClassName,
                    SectionName = (await _context.Sections.FindAsync(entity.SectionId))?.SectionName,
                    SubjectName = (await _context.Subjects.FindAsync(entity.SubjectId))?.SubjectName,
                    TeacherName = (await _context.Staffs.FindAsync(entity.StaffId))?.Name,
                    SchoolName = (await _context.Schools.FindAsync(entity.SchoolId))?.SchoolName,
                    AcademicYearName = (await _context.AcademicYears.FindAsync(entity.AcademicYearId))?.YearName
                };

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, response);
            }
            catch (Exception ex)
            {
                // ✅ Agar koi error aata hai toh usko log karo aur user ko message bhejo
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while saving data.", error = ex.Message });
            }
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClassSectionTeacherDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch.");

            try
            {
                var entity = await _context.ClassSectionTeachers.FindAsync(id);
                if (entity == null)
                    return NotFound();

                // Check duplicate for other records
                var duplicate = await _context.ClassSectionTeachers
                    .AnyAsync(cst =>
                        cst.Id != id &&
                        cst.ClassId == dto.ClassId &&
                        cst.SectionId == dto.SectionId &&
                        cst.StaffId == dto.StaffId &&
                        cst.SubjectId == dto.SubjectId &&
                        cst.SchoolId == dto.SchoolId &&
                        cst.AcademicYearId == dto.AcademicYearId);

                if (duplicate)
                    return Conflict("This mapping already exists for the academic year.");

                // Update
                entity.ClassId = dto.ClassId;
                entity.SectionId = dto.SectionId;
                entity.StaffId = dto.StaffId;
                entity.SubjectId = dto.SubjectId;
                entity.SchoolId = dto.SchoolId;
                entity.AcademicYearId = dto.AcademicYearId;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _context.ClassSectionTeachers.FindAsync(id);
                if (entity == null)
                    return NotFound();

                _context.ClassSectionTeachers.Remove(entity);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
