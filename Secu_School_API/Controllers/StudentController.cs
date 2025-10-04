using System;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Secu_School_API.Data;
using Secu_School_API.DTOs;
using Secu_School_API.Models;

namespace Secu_School_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly SchoolDbContext _context;

        public StudentController(SchoolDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromForm] StudentEnrollmentDto dto)
        {
            try
            {
                // Debug log the received DTO values (careful with sensitive data in production)
                Console.WriteLine($"Received create request: {System.Text.Json.JsonSerializer.Serialize(dto)}");

                string fileName = await SaveProfilePhoto(dto.ProfilePhoto);

                var student = new StudentEnrollment
                {
                    SchoolId = dto.SchoolId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth,
                    ClassId = dto.ClassId,
                    SectionId = dto.SectionId,
                    RollNumber = dto.RollNumber,
                    AdmissionDate = dto.AdmissionDate ?? DateTime.Now,
                    Status = dto.Status,
                    ProfilePhoto = fileName,
                    ParentContact = dto.ParentContact,
                    Address = dto.Address,
                    IsDeleted = 0
                };

                _context.StudentEnrollments.Add(student);
                await _context.SaveChangesAsync();
                // Create Login entry (tbl_login)
                var login = new UserLogin
                {
                    UserName = dto.ParentContact!, // Can be any unique username rule
                    PasswordHash = GenerateRandomPassword(), // Implement a proper password generator
                    UserType = "Student",
                    RefId = student.StudentId,  // Correct RefId now
                    SchoolId = dto.SchoolId
                };

                _context.Logins.Add(login);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Student created successfully", student });
            }
            catch (Exception ex)
            {
                // Log the error details to console (extend to file or monitoring system in production)
                Console.WriteLine($"Error creating student: {ex}");

                return BadRequest(new
                {
                    message = "Error creating student.",
                    error = ex.Message,
                    //details = ex.InnerException?.Message
                    full = ex.ToString()
                });
            }
        }
        private string GenerateRandomPassword()
        {
            // For demonstration, return a GUID. Replace with a proper secure password generator in real apps
            return Guid.NewGuid().ToString("N").Substring(0, 8); // 8-character random string
        }

        [HttpPut("update-student")]
        public async Task<IActionResult> UpdateStudent([FromForm] StudentEnrollmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.StudentId == null)
                return BadRequest("Student ID is required.");

            try
            {
                var student = await _context.StudentEnrollments.FindAsync(dto.StudentId.Value);
                if (student == null)
                    return NotFound("Student not found.");

                string fileName = student.ProfilePhoto!;
                if (dto.ProfilePhoto != null && dto.ProfilePhoto.FileName != "")
                {
                    fileName = await SaveProfilePhoto(dto.ProfilePhoto);
                }

                student.SchoolId = dto.SchoolId;
                student.FirstName = dto.FirstName;
                student.LastName = dto.LastName;
                student.Gender = dto.Gender;
                student.DateOfBirth = dto.DateOfBirth;
                student.ClassId = dto.ClassId;
                student.SectionId = dto.SectionId;
                student.RollNumber = dto.RollNumber;
                student.AdmissionDate = dto.AdmissionDate ?? student.AdmissionDate;
                student.Status = dto.Status;
                student.ProfilePhoto = fileName;
                student.ParentContact = dto.ParentContact;
                student.Address = dto.Address;
                student.IsDeleted = 0;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Student updated successfully", student });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating student.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                //var students = await _context.tbl_student
                //    .Where(s => s.isdeleted == 0)
                //    .ToListAsync();
                var students = await _context.StudentEnrollments
                    .Where(s => s.IsDeleted == 0)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.FirstName ,
                        s.LastName,
                        s.ProfilePhoto,
                        SectionName = s.Section!.SectionName, // Assuming nav property is 'Section'
                        ClassName = s.Class!.ClassName,  
                        s.RollNumber,
                        s.Gender,
                        s.ParentContact,
                        s.Address,
                        s.Status
                    }).ToListAsync();

                return Ok(students);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving students", error = ex.Message });
            }
        }

        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            try
            {
                var student = await _context.StudentEnrollments.FindAsync(id);
                if (student == null)
                    return NotFound("Student not found.");

                student.IsDeleted = 1;
                _context.StudentEnrollments.Update(student);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Student deleted successfully", student });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error deleting student.", error = ex.Message });
            }
        }

        [HttpGet("sections-by-class/{classId}/{schoolId}")]
        public async Task<IActionResult> GetSectionsByClass(int classId,  int schoolId)
        {
            var sections = await _context.Sections
                .Where(s => s.ClassId == classId)
                .Select(s => new
                {
                    id = s.SectionId,
                    sectionName = s.SectionName
                })
                .ToListAsync();

            return Ok(sections);
        }

        private async Task<string> SaveProfilePhoto(IFormFile file)
        {
            if (file == null) return null;

            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}